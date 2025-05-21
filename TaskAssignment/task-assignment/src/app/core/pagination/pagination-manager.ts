import { BehaviorSubject, Observable, of } from 'rxjs';
import { tap, map } from 'rxjs/operators';

export interface PageResponse<T> {
    items: T[];
    totalPages: number;
}

export class PaginationManager<T> {
    private currentPage = 0;
    private totalPages = Number.MAX_SAFE_INTEGER;
    private readonly cache = new Map<number, T[]>();
    private readonly pageSubject = new BehaviorSubject<T[]>([]);

    constructor(
        private readonly fetchPageFn: (page: number) => Observable<PageResponse<T>>,
        private readonly pageSize: number = 10
    ) { }

    get page$(): Observable<T[]> {
        return this.pageSubject.asObservable();
    }

    getCurrentPageItems(): T[] {
        return this.cache.get(this.currentPage) ?? [];
    }

    nextPage(): void {
        const reqPage = this.currentPage + 1;
        if (reqPage >= this.totalPages) return;

        this.currentPage++;
        this.loadCurrentPage();
    }

    prevPage(): void {
        if (this.currentPage > 0) {
            this.currentPage--;
            this.loadCurrentPage();
        }
    }

    reset(): void {
        this.currentPage = 0;
        this.loadCurrentPage();
    }

    invalidateCache(): void {
        this.cache.clear();
        this.totalPages = Number.MAX_SAFE_INTEGER;
    }

    updateItem(predicate: (item: T) => boolean, updater: (item: T) => T): void {
        const cached = this.cache.get(this.currentPage);
        if (!cached) return;

        const updated = cached.map(item => predicate(item) ? updater(item) : item);
        this.cache.set(this.currentPage, updated);
        this.pageSubject.next(updated);
    }

    private loadCurrentPage(): void {
        const page = this.currentPage;

        if (this.cache.has(page)) {
            this.pageSubject.next(this.cache.get(page)!);
            return;
        }

        this.fetchPageFn(page).pipe(
            tap(response => {
                this.cache.set(page, response.items);
                this.totalPages = response.totalPages;
            }),
            map(response => response.items)
        ).subscribe(items => {
            this.pageSubject.next(items);
        });
    }
}

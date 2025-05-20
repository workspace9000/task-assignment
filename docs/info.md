Lista reguł 

R1  Zadanie może być przypisane tylko do jednego użytkownika 
R2  Użytkownik musi mieć przypisanych od 5 do 11 zadań       
R3  Developer może mieć tylko zadania typu Implementacja     
R4  DevOps i Administrator mogą mieć wszystkie typy zadań    
R5  Poziom trudności 4–5: 10–30%                             
R6  Poziom trudności 1–2: max 50%                            
R7  Poziom trudności 3: max 90%     

Decyzje Architektury itp

Monolit                                                                       
DDD z CQRS                                                                    
Warstwy logiczne: Api, Application, Domain, Infrastructure                    
Vertical Slice Architecture – kod grupowany według funkcjonalności biznesowej 
Command: MediatR + handlery
Unit of Work + transakcje w pipeline MediatR  
EF InMemory                                
Walidacja na frontend i backend
Za każdym razem przy zatwierdzeniu są przesyłane wszystkie zadania usera

Angular 18 (ale ze starym podejściem: moduły, bez sygnałów, komponenty importowane-nie standalone)

Uwagi

Brakuje ustabilizowania pod kątem współbieżności
Brak monitoringu logowania
Brak autoryzacji
Brak testów jednostkowych
Z braku czasu nie zrobiłem stronicowania na frontendzie (jest stronicowanie na BE)
Brak stylowania na froncie i brak podejścia UX (frontend ubogi; nie wyświetla np szczegółów zadania; brak w zadaniu tytuł)- głównie motoryka zrobiona
Brak standardu i przechwytywania błędów z backendu
Na backend niektóre obszary wymagają refaktoryzacji (lepsze rozlokowanie kodu wg warstw; enkapsulacja reguł biznesowych po stronie Query; walidacja modeli API)
Backend: enumy jako stringi dla uproszczenia i czytelności
Podział ról na DevOps, Administrator
Frontend: url na sztywno
Frontend: brak zabezpieczenia przed wyjściem z formularza po wprowadzonych zmianach

W katalogu jest diagram z modelem dziedziny


W razie spotkania chętnie omówię te i inne kwestie/ decyzje w projekcie.

Pozdrawiam
Tomasz Broniewski
                       
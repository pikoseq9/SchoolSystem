# SchoolSystem

**SchoolSystem** to aplikacja typu desktop dla systemu zarządzania szkołą, napisana w języku C# z wykorzystaniem WPF (Windows Presentation Foundation). Projekt umożliwia zarządzanie informacjami o uczniach, nauczycielach, ocenach i planach lekcji w przejrzysty i zorganizowany sposób.

## Główne funkcjonalności

- **Logowanie użytkowników** – Obsługa logowania uczniów oraz nauczycieli z rozróżnieniem typów kont.
- **Panele użytkowników** – Dedykowane dashboardy dla ucznia, nauczyciela oraz dyrektora szkoły.
- **Zarządzanie uczniami i nauczycielami** – Przeglądanie, wyszukiwanie oraz zarządzanie danymi osobowymi uczniów i nauczycieli.
- **Wyświetlanie ocen** – Możliwość przeglądania ocen wraz z kategoriami i wagą, zarówno ogólnie jak i według ucznia.
- **Wyświetlanie planu lekcji** – Dostęp do planów zajęć w formie przejrzystych stron.
- **Uwagi i komunikaty** – Przeglądanie oraz zarządzanie uwagami dotyczącymi uczniów.
- **Obsługa błędów** – Rozbudowane mechanizmy obsługi wyjątków i komunikatów błędów, zwłaszcza przy operacjach na bazie danych.

## Struktura projektu - MVVM

- `View/` – Warstwa widoku (WPF XAML + code-behind), dashboardy i podstrony (np. oceny, plan lekcji, uwagi).
- `Repositiories/` – Repozytoria odpowiedzialne za komunikację z bazą danych SQLite (uczniowie, nauczyciele, oceny).
- `ViewModel/` – Logika biznesowa oraz obsługa akcji użytkownika (np. logowanie).

## Wymagania

- .NET (zalecana najnowsza wersja .NET Core lub .NET Framework zgodna z projektem WPF)
- SQLite jako system bazy danych

## Uruchomienie

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/pikoseq9/SchoolSystem.git
   ```
2. Otwórz projekt w Visual Studio lub innym IDE obsługującym C# i WPF.
3. Przygotuj bazę danych SQLite (jeśli nie jest dołączona do repozytorium).
4. Uruchom projekt.

## Autor

- [pikoseq9](https://github.com/pikoseq9)

## Licencja

Projekt udostępniony na licencji MIT (lub innej, jeśli została określona).

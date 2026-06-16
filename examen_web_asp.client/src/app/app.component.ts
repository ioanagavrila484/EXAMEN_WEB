import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

// Forma datelor pe care le primim de la backend dupa login.
interface LoginResult {
  id: number;
  userName: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  public username = '';                              // legat de input prin [(ngModel)]
  public loggedInUser: LoginResult | null = null;    // userul logat (null = nelogat)
  public errorMessage = '';

  constructor(private http: HttpClient) {}

  // ====================== LOGIN ======================
  login() {
    this.errorMessage = '';

    // POST catre /api/auth/login?userName=... cu numele DIRECT in URL (query string).
    // Fara body, fara JSON.stringify, fara ghilimele. encodeURIComponent => merge si cu spatii.
    // URL-ul trebuie sa fie IDENTIC cu [Route]/[HttpPost] din AuthController.cs
    this.http.post<LoginResult>(`/api/auth/login?userName=${encodeURIComponent(this.username)}`, null).subscribe(
      (result: LoginResult) => {
        this.loggedInUser = result; // login reusit -> salvam userul
      },
      (error: any) => {
        this.errorMessage = 'Utilizator inexistent';
        console.error(error);
      }
    );
  }

  /*
  =====================================================================
    SINTEZA LEGATURA FE <-> BE (TypeScript)
  =====================================================================
    Aici faci legatura cu backend-ul prin HttpClient.
    URL-ul incepe cu '/api/<ruta>' si trebuie sa fie IDENTIC cu
    [Route] / [HttpGet/Post/Put] din controllerul C#.

    Pasi:
      1) injectezi HttpClient in constructor (deja facut mai sus).
      2) apelezi this.http.get/post/put(...).subscribe(success, error).
      3) salvezi raspunsul intr-o proprietate ca sa-l afisezi in HTML.

    ---- TEMPLATE GET (citeste lista din BE) -------------------------
    utilizatori: any[] = [];

    getUtilizatori() {
      this.http.get<any[]>('/api/auth/all').subscribe(
        (result: any[]) => { this.utilizatori = result; },
        (error: any) => { console.error(error); }
      );
    }

    ---- TEMPLATE POST (creeaza item nou in BE) ----------------------
    newUser: any = { userName: '' };

    createUser() {
      this.http.post('/api/auth/create', this.newUser).subscribe(
        () => { this.getUtilizatori(); },   // reincarca lista dupa adaugare
        (error: any) => { console.error(error); }
      );
    }

    ---- TEMPLATE PUT (update item existent in BE) -------------------
    editUser: any = { id: 0, userName: '' };

    updateUser() {
      this.http.put(`/api/auth/${this.editUser.id}`, this.editUser).subscribe(
        () => { this.getUtilizatori(); },
        (error: any) => { console.error(error); }
      );
    }
  =====================================================================
  */

  title = 'examen_web_asp.client';
}

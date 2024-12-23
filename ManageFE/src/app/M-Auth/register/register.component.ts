import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { ToastModule } from 'primeng/toast';
import { AuthService } from '../../M-Services';
import { RegisterRequest, RegisterResponse } from '../../M-Models/Register';
import { LoginRequest } from '../../M-Models/Login';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, ReactiveFormsModule, ToastModule, MessagesModule],
  providers: [MessageService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  numberYearCurrent!: number;
  numberYearNext!: number;

  registerForm!: FormGroup;

  credentials: RegisterRequest = {
    email: '',
    password: ''
  };

  credentialsLogin: LoginRequest = {
    email: '',
    password: ''
  };

  constructor(private fb: FormBuilder,
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.messageService.clear();

    this.numberYearCurrent = new Date().getFullYear();
    this.numberYearNext = new Date().getFullYear() + 1;

    this.registerForm = this.setForm();
  }

  onGoToLogin() {
    this.router.navigate(['Login']); 
  }

  onRegister() {
    this.getFormValue();

    this.register();
  }

  private setForm(): FormGroup {
    return this.fb.group({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
    
  private getFormValue() {

    this.credentials = {
      email: this.registerForm.get("email")?.value,
      password: this.registerForm.get("password")?.value,
    }

    console.log('getFormValue -> ', this.credentials);
  }

  private register() {
    this.authService.register(this.credentials).subscribe({
      next: (response: RegisterResponse) => {
        if (response.status === 200) {

          this.credentialsLogin.email = this.credentials.email;
          this.credentialsLogin.password = this.credentials.password

          this.login();

          this.messageService.clear();
          this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Registrazione al sistema effettuata con successo!' })

        } else {

          const passwordErrorMessages = response.detail.split(','); // Divido gli errori in una lista
          const translatedErrors: string[] = [];

          passwordErrorMessages.forEach(errorMessage => {
            if (errorMessage.includes('Passwords must be at least 6 characters')) {
              translatedErrors.push('La password deve essere lunga almeno 6 caratteri');
            } else if (errorMessage.includes('Passwords must have at least one non alphanumeric character')) {
              translatedErrors.push('La password deve contenere almeno un carattere non alfanumerico');
            } else if (errorMessage.includes('Passwords must have at least one digit')) {
              translatedErrors.push('La password deve contenere almeno un numero');
            } else if (errorMessage.includes('Passwords must have at least one uppercase')) {
              translatedErrors.push('La password deve contenere almeno una lettera maiuscola');
            } else if (errorMessage.includes('Passwords must have at least one lowercase')) {
              translatedErrors.push('La password deve contenere almeno una lettera minuscola');
            } else if (errorMessage.includes('is already taken.')) {
              translatedErrors.push('Email presente nel sistema, accedi o contatta l\'assistenza se non sei stato tu a registrarti');
            } else {
              translatedErrors.push(errorMessage);  // Se non troviamo una corrispondenza, manteniamo il messaggio originale
            }
          });

          // Unisco gli errori tradotti in una stringa separata da virgola
          let translatedMessage = translatedErrors.join(', ');

          // Cambia le virgole con i punti e aggiunge uno spazio dopo ogni punto
          translatedMessage = translatedMessage.replace(/,/g, '.');  // Cambia tutte le virgole con i punti
          translatedMessage = translatedMessage.replace(/\./g, '. '); // Aggiungi uno spazio dopo ogni punto

          // Rimuovo l'ultimo spazio extra (se presente)
          translatedMessage = translatedMessage.trim();

          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la registrazione al sistema. \n\n ' + translatedMessage })
          alert(`Registration failed: ${translatedMessage}`);

        }
      },
      error: err => {
        if (err.error && err.error.errors) {
          const errorMessages = Object.values(err.error.errors).flat().join(', ');

          const passwordErrorMessages = errorMessages.split(','); // Divido gli errori in una lista
          const translatedErrors: string[] = [];

          passwordErrorMessages.forEach(errorMessage => {
            if (errorMessage.includes('Passwords must be at least 6 characters')) {
              translatedErrors.push('La password deve essere lunga almeno 6 caratteri');
            } else if (errorMessage.includes('Passwords must have at least one non alphanumeric character')) {
              translatedErrors.push('La password deve contenere almeno un carattere non alfanumerico');
            } else if (errorMessage.includes('Passwords must have at least one digit')) {
              translatedErrors.push('La password deve contenere almeno un numero');
            } else if (errorMessage.includes('Passwords must have at least one uppercase')) {
              translatedErrors.push('La password deve contenere almeno una lettera maiuscola');
            } else if (errorMessage.includes('Passwords must have at least one lowercase')) {
              translatedErrors.push('La password deve contenere almeno una lettera minuscola');
            } else if (errorMessage.includes('is already taken.')) {
              translatedErrors.push('Email presente nel sistema, accedi o contatta l\'assistenza se non sei stato tu a registrarti');
            } else {
              translatedErrors.push(errorMessage);  // Se non troviamo una corrispondenza, manteniamo il messaggio originale
            }
          });

          // Unisco gli errori tradotti in una stringa separata da virgola
          let translatedMessage = translatedErrors.join(', ');

          // Cambia le virgole con i punti e aggiunge uno spazio dopo ogni punto
          translatedMessage = translatedMessage.replace(/,/g, '.');  // Cambia tutte le virgole con i punti
          translatedMessage = translatedMessage.replace(/\./g, '. '); // Aggiungi uno spazio dopo ogni punto

          // Rimuovo l'ultimo spazio extra (se presente)
          translatedMessage = translatedMessage.trim();

          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la registrazione al sistema. \n ' + translatedMessage })
        } else {

          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la registrazione al sistema. Per favore riprovare' })

          alert('Registration failed. Please try again.');
        }
      }
    });
  }

  private login() {
    this.authService.login(this.credentialsLogin)
      .subscribe(() => {
        console.log('Login successful')

        if (this.isLoggedIn()) {    
          this.messageService.clear();
          this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Accesso al sistema effettuato con successo!' })

          this.router.navigate(['Documenti']); 
        }
        else {
          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante l\'accesso al sistema, controllare le credenziali inserite' })
        }

      })
  }

  private isLoggedIn() {
    return this.authService.isLoggedIn();
  }

}

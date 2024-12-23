import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { ToastModule } from 'primeng/toast';
import { AuthService } from '../../M-Services';
import { LoginRequest } from '../../M-Models/Login';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, ReactiveFormsModule, ToastModule, MessagesModule],
  providers: [MessageService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  numberYearCurrent!: number;
  numberYearNext!: number;

  loginForm!: FormGroup;

  credentials: LoginRequest = {
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

    this.loginForm = this.setForm();

  }

  onGoToRegister() {
    this.router.navigate(['Register']); 
  }

  onLogin() {
    this.getFormValue();

    this.login();
  }

  private setForm(): FormGroup {
    return this.fb.group({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  private getFormValue() {

    this.credentials = {
      email: this.loginForm.get("email")?.value,
      password: this.loginForm.get("password")?.value,
    }

    console.log('getFormValue -> ', this.credentials);
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  login() {
    this.authService.login(this.credentials)
      .subscribe(() => {
        console.log('Login successful')

        if (this.isLoggedIn()) {    
          this.messageService.clear();
          this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Accesso al sistema effettuato con successo!' })

          this.router.navigate(['Home']); 
        }
        else {
          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante l\'accesso al sistema, controllare le credenziali inserite' })
        }

      })
  }

}

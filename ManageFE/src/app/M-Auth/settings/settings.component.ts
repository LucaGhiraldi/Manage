import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { MessagesModule } from 'primeng/messages';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { PanelModule } from 'primeng/panel';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AuthService } from '../../M-Services';
import { UserInfo } from '../../M-Models/UserInfo';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, FloatLabelModule, DropdownModule, ButtonModule, MessagesModule, InputTextModule, InputGroupModule, InputGroupAddonModule, PanelModule, ConfirmDialogModule, ToastModule],
  providers: [MessageService, ConfirmationService],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {

  messaggioVerificaEmailConferma = [ 
    { 
      severity: "success", 
      detail: "La tua email è stata confermata. Il profilo è verificato.", 
    }, 
  ]; 
  messaggioVerificaEmailErrore = [ 
    { 
      severity: "warn", 
      detail: "La tua email non è stata confermata. Non è possibile verificare il profilo.", 
    }, 
  ]; 

  User: UserInfo = { email: '', isEmailConfirmed: false };

  constructor(private messageService: MessageService,
              private confirmationService: ConfirmationService,
              private authService: AuthService) {}

  ngOnInit(): void {
    this.GetUserInfo();
  }

  private GetUserInfo() {
    this.authService.info().subscribe({
      next: (response: UserInfo) => {
        this.User = response
      },
      error: err => {

      }
    });

  }

}

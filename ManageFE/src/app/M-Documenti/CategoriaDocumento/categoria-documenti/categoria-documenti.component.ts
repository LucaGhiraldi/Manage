import { CommonModule, Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';

import { TableModule } from 'primeng/table';
import { CategoriaDocumentiCreateData, CategoriaDocumentiData, CategoriaDocumentiUpdateData } from '../../../M-Models/CategoriaDocumenti';
import { CategoriaDocumentiService } from '../../../M-Services';
import { ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DialogModule } from 'primeng/dialog';
import { MessagesModule } from 'primeng/messages';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CategoriaDocumentiDetailComponent } from "../categoria-documenti-detail/categoria-documenti-detail.component";

@Component({
  selector: 'app-categoria-documenti',
  standalone: true,
  imports: [HttpClientModule, TableModule, CommonModule, FormsModule, ReactiveFormsModule, ButtonModule, CalendarModule, ToolbarModule, ToastModule, DialogModule, MessagesModule, InputTextModule, ConfirmDialogModule, CategoriaDocumentiDetailComponent],
  providers: [CategoriaDocumentiService, MessageService, ConfirmationService],
  templateUrl: './categoria-documenti.component.html',
  styleUrl: './categoria-documenti.component.scss'
})

export class CategoriaDocumentiComponent implements OnInit {

  // Id Categoria
  idCategoria: number = 0;
  idCategoriaDelete: number = 0;
  // Array categorie
  categoriesValue: CategoriaDocumentiData[] = [];
  updateCategoryValue!: CategoriaDocumentiUpdateData;
  createCategoryValue!: CategoriaDocumentiCreateData;
  // Variabile form Categoria
  // categoryForm!: FormGroup;

  // Oggetto categoria per create e modify
  categoryValue!: CategoriaDocumentiData; 

  titoloDialog!: string;

  messaggioCaricamento = [ 
    { 
      severity: "info", 
      summary: "Caricamento", 
      detail: "Caricamento dei dati in corso... Per favore attendere", 
    }, 
  ]; 
  messaggioErrore = [ 
    { 
      severity: "error", 
      summary: "Errore", 
      detail: "Problemi nell'acquisizione dei dati delle categorie...", 
    }, 
  ]; 
  loading: boolean = true;  // Stato del caricamento
  successLoading = false;   // Risultato del caricamento
  visibleInsertModifyCategory: boolean = false;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private location: Location,
              private messageService: MessageService,
              private confirmationService: ConfirmationService,
              private categoriaDocumentiService: CategoriaDocumentiService) { }

  ngOnInit(): void {
    this.messageService.clear();

    // this.categoryForm = this.setForm();

    this.getCategoriaDocumenti();
  }

  // Get All Categorie
  getCategoriaDocumenti(): void {
    this.categoriaDocumentiService.getAllCategoriaDocumenti().subscribe({
      next: (data: CategoriaDocumentiData[]) => {
        this.categoriesValue = data;  // Memorizza il risultato nell'array
        console.log(this.categoriesValue);  // Debug: logga i documenti in console

        this.loading = false;  // Imposta lo stato di caricamento a false
        this.successLoading = true;
      },
      error: (err) => {
        this.loading = false;  // Anche in caso di errore, imposta lo stato di caricamento a false
        this.successLoading = false;

        console.error('Errore nel recupero delle categorie documenti', err);
      }
    });
  }

  goBack() {
    this.location.back(); // Torna alla rotta precedente
  }

  onGoToNewCategoryDialog() {
    this.titoloDialog = "Inserimento nuova categoria";
    this.idCategoria = 0;
    this.visibleInsertModifyCategory = true;
    
    // this.categoryForm = this.setForm();
  }

  onGoToModifyCategoryDialog(idCategory: number) {
    this.titoloDialog = "Modifica categoria";
    this.idCategoria = idCategory;

    var categoryDataValue = this.categoriesValue.find(c => c.id === idCategory);
    if (categoryDataValue !== undefined)
      this.categoryValue = categoryDataValue;
    
    this.visibleInsertModifyCategory = true;

    // this.idCategoria = idCategory;

    // this.setFormValue();
  }

  onCloseDialog(event: boolean) {
    this.visibleInsertModifyCategory = event;
    this.idCategoriaDelete = 0;
  }

  onSaveCategory(event: CategoriaDocumentiData) {
    if (this.idCategoria > 0) {

      this.updateCategoryValue = {
        id: event.id,
        nomeCategoria: event.nomeCategoria,
        descrizioneCategoria: event.descrizioneCategoria,
        dataInserimentoCategoria: event.dataInserimentoCategoria,
        sottoCategorie: event.sottoCategorie
      }

      // this.getFormValue();

      // Modify categoria "oggetto: updateCategoryValue"
      this.updateCategory();
    }
    else {

      this.createCategoryValue = {
        nomeCategoria: event.nomeCategoria,
        descrizioneCategoria: event.descrizioneCategoria,
        dataInserimentoCategoria: event.dataInserimentoCategoria,
        sottoCategorie: event.sottoCategorie
      }

      // this.getFormValueCreate();

      // Create categoria "oggetto: createCategoryValue"
      this.createCategory();
    }
  }

  onOpenDialogDelete(event: Event, idCategory: number) {

    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Vuoi eliminare questa categoria definitivamente?',
      header: 'Conferma cancellazione categoria',
      icon: 'fa-solid fa-circle-exclamation',
      acceptButtonStyleClass:"p-button-danger p-button-text",
      rejectButtonStyleClass:"p-button-text p-button-text",
      acceptIcon:"none",
      rejectIcon:"none",

      accept: () => {
        this.idCategoriaDelete = idCategory;
        this.onDeleteCategory();
      },
      reject: () => {
        this.messageService.add({ severity: 'info', summary: 'Indietro', detail: 'Annullata la procedura di eliminazione della categoria' });
      }
    });

  }

  // onCloseDialogDelete() {
  //   // this.visibleDelete = false;
  //   this.idCategoriaDelete = 0;
  // }

  onDeleteCategory() {
    this.deleteCategory(this.idCategoriaDelete);
  }

  // Set Form
  // private setForm(): FormGroup {
  //   return this.fb.group({
  //     nomeCategoria: new FormControl('', Validators.required),
  //     descrizioneCategoria: new FormControl('', Validators.required),
  //     dataInserimentoCategoria: new FormControl({ value: new Date(), disabled: true }),
  //     isPredefinita: new FormControl({ value: false, disabled: true }), // booleano originale
  //   });
  // }

  // private setFormValue(): void {
  //   this.categoryForm.setValue({
  //     nomeCategoria: this.categoryValue?.nomeCategoria,
  //     descrizioneCategoria: this.categoryValue?.descrizioneCategoria,
  //     dataInserimentoCategoria: new Date(this.categoryValue?.dataInserimentoCategoria),
  //     isPredefinita: this.categoryValue?.isPredefinita,
  //   })
  // }

  // Get From
  // private getFormValue() {

  //   this.updateCategoryValue = {
  //     id: this.idCategoria,
  //     nomeCategoria: this.categoryForm.get("nomeCategoria")?.value,
  //     descrizioneCategoria: this.categoryForm.get("descrizioneCategoria")?.value,
  //     dataInserimentoCategoria: this.categoryForm.get("dataInserimentoCategoria")?.value,
  //     isPredefinita: false,
  //   }

  //   console.log('getFormValue -> ', this.updateCategoryValue);
  // }

  // private getFormValueCreate() {

  //   this.createCategoryValue = {
  //     nomeCategoria: this.categoryForm.get("nomeCategoria")?.value,
  //     descrizioneCategoria: this.categoryForm.get("descrizioneCategoria")?.value,
  //     dataInserimentoCategoria: this.categoryForm.get("dataInserimentoCategoria")?.value,
  //     isPredefinita: false,
  //   }

  //   console.log('getFormValueCreate -> ', this.createCategoryValue);
  // }

  // Create categoria
  private createCategory() {
    this.categoriaDocumentiService.CreateCategory(this.createCategoryValue).subscribe({
      next: (data: CategoriaDocumentiData) => {
        this.categoryValue = data;  // Memorizza il risultato nell'array
        console.log('createDocumento -> ', this.categoryValue);  // Debug: logga i documenti in console
        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Creazione della categoria effettuata con successo!' })

        // Richiamo il get delle categorie
        this.getCategoriaDocumenti();

        this.visibleInsertModifyCategory = false;
      },
      error: (err) => {
        console.error('Errore nell\' inserimento della categoria', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di creazione della categoria' })
      }
    });
  }

  // Modify categoria
  private updateCategory() {
    this.categoriaDocumentiService.UpdateCategory(this.updateCategoryValue).subscribe({
      next: (data: CategoriaDocumentiData) => {
        this.categoryValue = data;  // Memorizza il risultato nell'array
        console.log('updateCategory -> ', this.categoryValue);  // Debug: logga i documenti in console
        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Modifica della categoria effettuata con successo!' })

        // Richiamo il get delle categorie
        this.getCategoriaDocumenti();

        this.visibleInsertModifyCategory = false;
      },
      error: (err) => {
        console.error('Errore nella modifica della categoria', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di modifica della categoria' })
      }
    });
  }

  // Delete categoria
  private deleteCategory(idCategoria: number) {
    this.categoriaDocumentiService.DeleteCategory(idCategoria).subscribe({
      next: (data: boolean) => {
        var result = data;
        if (result) {
          this.messageService.clear();
          this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Eliminazione della categoria effettuata con successo!' })

          // Richiamo il get delle categorie
          this.getCategoriaDocumenti();

          // this.visibleDelete = false;
        }
        else {
          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Impossibile eliminare la categoria.' })

          // this.visibleDelete = false;
        }
      },
      error: (err) => {

        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'La categoria collegata ad un documento non può essere eliminata.' })

        // this.visibleDelete = false;
      }
    });
  }
}

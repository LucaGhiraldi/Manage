import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { MessagesModule } from 'primeng/messages';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CategoriaDocumentiService } from '../../../M-Services';
import { CalendarModule } from 'primeng/calendar';
import { CategoriaDocumentiData, SottoCategoriaDocumentiData } from '../../../M-Models/CategoriaDocumenti';

@Component({
  selector: 'app-categoria-documenti-detail',
  standalone: true,
  imports: [HttpClientModule, TableModule, CommonModule, FormsModule, CalendarModule, ReactiveFormsModule, ButtonModule, ToastModule, MessagesModule, InputTextModule, ConfirmDialogModule],
  providers: [CategoriaDocumentiService, MessageService, ConfirmationService],
  templateUrl: './categoria-documenti-detail.component.html',
  styleUrl: './categoria-documenti-detail.component.scss'
})
export class CategoriaDocumentiDetailComponent implements OnInit, OnChanges {

  @Input() categoryValueData!: CategoriaDocumentiData;
  @Input() idCategoria!: number;
  // Output: Eventi di salvataggio e chiusura
  @Output() 
  onSaveCategoryValue = new EventEmitter<CategoriaDocumentiData>();
  @Output() 
  onCloseDialogValue = new EventEmitter<boolean>();

  categoriyValueResponse!: CategoriaDocumentiData;
  sottoCategoriesValueResponse: SottoCategoriaDocumentiData[] = [];
  newSottoCategoriesValueResponse!: SottoCategoriaDocumentiData;
  idSubCategoryDelete: number = 0;
  listaIdSottoCategorie: number[] = [];

  categoryForm!: FormGroup;
  
  clonedProducts: { [s: number]: SottoCategoriaDocumentiData } = {};

  constructor(private fb: FormBuilder,
              private messageService: MessageService,
              private confirmationService: ConfirmationService) { }

  ngOnInit(): void {
    this.categoryForm = this.setForm();
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['idCategoria']) {
      this.idCategoria = changes['idCategoria'].currentValue;
      if (this.idCategoria > 0) {
        this.setFormValue();
        this.categoriyValueResponse = this.categoryValueData;
        this.sottoCategoriesValueResponse = this.categoryValueData.sottoCategorie;
      }
      else {
        this.categoryForm = this.setForm();
        this.sottoCategoriesValueResponse = [];
      }
    }

    if (changes['categoryValueData']) {
      this.categoryValueData = changes['categoryValueData'].currentValue
      if (this.idCategoria > 0) {
        this.setFormValue();
        this.categoriyValueResponse = this.categoryValueData;
        this.sottoCategoriesValueResponse = this.categoryValueData.sottoCategorie;
      }
      else {
        this.categoryForm = this.setForm();
        this.sottoCategoriesValueResponse = [];
      }
    }

  }

  onCreateNewSottoCategoria() {
    // this.categoriyValueData.sottoCategorie.unshift();

    const timestamp = new Date().getTime();
    const uniqueId = timestamp + Math.floor(Math.random() * 1000);  // Genera un ID unico

    this.listaIdSottoCategorie.push(uniqueId);

    this.newSottoCategoriesValueResponse = {
      id: uniqueId,
      nomeSottoCategoria: '',
      descrizioneSottoCategoria: '',
      dataInserimentoSottoCategoria: new Date()
    }
    this.sottoCategoriesValueResponse.unshift(this.newSottoCategoriesValueResponse);

    // this.sottoCategoriesValueResponse[this.newSottoCategoriesValueResponse.id] = { ...this.newSottoCategoriesValueResponse}
  }

  onOpenDialogDeleteSubCategory(event: Event, idSottoCategria: number) {
    // this.visibleDeleteFile = true;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Vuoi eliminare questa sotto categoria?',
      header: 'Conferma cancellazione sotto categoria',
      icon: 'fa-solid fa-circle-exclamation',
      acceptButtonStyleClass:"p-button-danger p-button-text",
      rejectButtonStyleClass:"p-button-text p-button-text",
      acceptIcon:"none",
      rejectIcon:"none",

      accept: () => {
        this.idSubCategoryDelete = idSottoCategria;
        this.onDeleteSuCategory();
      },
      reject: () => {
        this.messageService.add({ severity: 'info', summary: 'Indietro', detail: 'Annullata la procedura di eliminazione della sotto categoria' });
      }
    });
  }

  // Metodi per emettere gli eventi
  onSaveCategory() {

    // Se ci sono sotto categorie eseguo l'inserimento dei dati (invio al client)
    if (this.sottoCategoriesValueResponse.length > 0) {

      // Acquisire i dati del form ed inviarli al padre
      this.getFormValue();

      // Cambiare gli id delle nuove sotto categorie con 0
      this.categoriyValueResponse.sottoCategorie.forEach(subCat => {
        
        // per tutti gli id delle sottocategorie
        // Cerco l'indice del valore
        var index = this.listaIdSottoCategorie.findIndex(idNew => idNew === subCat.id);
        // se presente, quindi è maggiore di 0
        if (index >= 0) 
        {
          // Cambio il valore dell'id
          subCat.id = 0;
        }        
      });

      console.log("this.categoriyValueResponse.sottoCategorie - Dopo modifiche id -> ", this.categoriyValueResponse.sottoCategorie);

      // Controllare se ci sono valori nulli, vuoti in riferimento alle sottocategorie
      var verifica = false;
      // Cambiare gli id delle nuove sotto categorie con 0
      this.categoriyValueResponse.sottoCategorie.forEach(subCat => {
  
        if (subCat.nomeSottoCategoria === undefined || 
            subCat.nomeSottoCategoria === null      || 
            subCat.nomeSottoCategoria === ""        || 
            subCat.nomeSottoCategoria === ''        ||
            subCat.descrizioneSottoCategoria === undefined || 
            subCat.descrizioneSottoCategoria === null      || 
            subCat.descrizioneSottoCategoria === ""        || 
            subCat.descrizioneSottoCategoria === '') {
              verifica = true;
        }

      });

      if (verifica) {
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Specificare i valori nella sotto categoria' });
      }
      else {
        // Inviare i dati al padre
        this.onSaveCategoryValue.emit(this.categoriyValueResponse); 
      }
    }
    else {
      this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Specificare almeno una sotto categoria' });
    }

  }

  onCloseDialog() {
    this.onCloseDialogValue.emit(false);
  }

  private onDeleteSuCategory() {
    // Eseguo la rimozione nell'array utilizzato in visualizzazione
    var index = this.sottoCategoriesValueResponse.findIndex(sc => sc.id === this.idSubCategoryDelete);
    this.sottoCategoriesValueResponse.splice(index, 1)

    // Se l'id da eliminare è presente nell'array degli id delle nuove sotto categorie, li elimino
    this.listaIdSottoCategorie.forEach(idSottoCategoria => {
      if (this.idSubCategoryDelete === idSottoCategoria) {
        var index = this.listaIdSottoCategorie.findIndex(id => id === this.idSubCategoryDelete);
        this.listaIdSottoCategorie.splice(index, 1)
      }
    });

    this.messageService.clear();
    this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Eliminazione della sotto categoria effettuata con successo!' })
  }

  private setForm(): FormGroup {
    return this.fb.group({
      // Per la categoria
      nomeCategoria: new FormControl('', Validators.required),
      descrizioneCategoria: new FormControl('', Validators.required),
      dataInserimentoCategoria: new FormControl({ value: new Date(), disabled: true }),
      // isPredefinita: new FormControl({ value: false, disabled: true }), // booleano originale
    });
  }

  private setFormValue(): void {
    this.categoryForm.setValue({
      nomeCategoria: this.categoryValueData?.nomeCategoria,
      descrizioneCategoria: this.categoryValueData?.descrizioneCategoria,
      dataInserimentoCategoria: new Date(this.categoryValueData?.dataInserimentoCategoria),
      // isPredefinita: this.categoryValueData?.isPredefinita,
    })
  }

  private getFormValue() {

    this.categoriyValueResponse = {
      id: this.idCategoria,
      nomeCategoria: this.categoryForm.get("nomeCategoria")?.value,
      descrizioneCategoria: this.categoryForm.get("descrizioneCategoria")?.value,
      dataInserimentoCategoria: this.categoryForm.get("dataInserimentoCategoria")?.value,
      isPredefinita: false,
      utenteId: '',
      sottoCategorie: this.sottoCategoriesValueResponse,
    }

    console.log('getFormValue -> ', this.categoriyValueResponse);
  }

}

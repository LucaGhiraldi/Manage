import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { InputSwitchModule } from 'primeng/inputswitch';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { CategoriaDocumentiData } from '../../../M-Models/CategoriaDocumenti';
import { CategoriaDocumentiService, DocumentiService, FileDocumentiService } from '../../../M-Services';
import { DocumentiCreateData, DocumentiData, DocumentiDataValue } from '../../../M-Models/Documenti';
import { FileDocumentiData, FileDocumentiDataValue } from '../../../M-Models/FileDocumenti';
import { DialogModule } from 'primeng/dialog';
import { FileUpload, FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService, SelectItemGroup } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-documento',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, ReactiveFormsModule, ButtonModule, PanelModule, TableModule, InputSwitchModule, InputTextModule, CalendarModule, DropdownModule, DialogModule, FileUploadModule, ToolbarModule, ToastModule, ConfirmDialogModule ],
  providers: [DocumentiService, CategoriaDocumentiService, FileDocumentiService, MessageService, ConfirmationService],
  templateUrl: './documento.component.html',
  styleUrl: './documento.component.scss'
})
export class DocumentoComponent implements OnInit {

  @ViewChild('fileUploader') fileUploader!: FileUpload;

  formDocumento!: FormGroup;

  idDocumento!: number;
  checked: boolean = false;
  categoriesValue: CategoriaDocumentiData[] = [];
  documento!: DocumentiData;

  filesValue: FileDocumentiData[] = [];
  filesValueBackup: FileDocumentiData[] = [];
  filesInserimento: File[] = [];

  // visibleDelete: boolean = false;
  idDocumentoDelete!: number;

  // visibleDeleteFile: boolean = false;
  idFileDelete!: number;

  uploadedFiles: FileDocumentiDataValue[] = [];
  idUnique: number[] = [];

  updateDocumentiValue!: DocumentiDataValue;
  createDocumentoValue!: DocumentiCreateData;
  resultDeleteDocumento: boolean = false;
  groupedCategorie: SelectItemGroup[] = [];

  loadingDocumento: boolean = true;  // Stato del caricamento
  successLoadingDocumento: boolean = false;   // Risultato del caricamento
  loadingFileDocumento: boolean = true;  // Stato del caricamento
  successLoadingFileDocumento: boolean = false;   // Risultato del caricamento

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
      detail: "Problemi nell'acquisizione dei dati...", 
    }, 
  ]; 

  constructor(private documentiService: DocumentiService,
              private categoriaDocumentiService: CategoriaDocumentiService,
              private fileDocumentiService: FileDocumentiService,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private messageService: MessageService,
              private confirmationService: ConfirmationService,
              private location: Location) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
        this.idDocumento = +params['id']; // Usa il + per convertire in numero
    });

    this.messageService.clear();

    this.resultDeleteDocumento = false;
    // this.visibleDelete = false;
    // this.visibleDeleteFile = false;

    if (this.idDocumento !== 0 && this.idDocumento !== undefined && this.idDocumento !== null) {
  
      this.formDocumento = this.setFormDatiGeneraliUpdate();
  
      // Get categorie
      this.getCategoriaDocumenti();
  
      // Get documento
      this.getDocumentoById(this.idDocumento);
    }
    else {

      this.loadingDocumento = false;  // Stato del caricamento
      this.successLoadingDocumento = true;   // Risultato del caricamento
      this.loadingFileDocumento = false;  // Stato del caricamento
      this.successLoadingFileDocumento = true;   // Risultato del caricamento

      this.formDocumento = this.setFormDatiGeneraliCreate();

      this.checked = true;

      // Get categorie
      this.getCategoriaDocumenti();
    }
    
  }

  // Abilita/disabilita i campi in base all'interruttore
  onCheckedChange(event: any): void {
    this.checked = event.checked;  // Aggiorna lo stato del switch

    if (this.checked) {
      this.formDocumento.enable();  // Abilita tutti i controlli
      this.formDocumento.get('DataInserimentoDocumento')?.disable();

    } else {
      this.formDocumento.disable();  // Disabilita tutti i controlli
    }
  }

  goBack() {
    this.location.back(); // Torna alla rotta precedente
  }

  // Get categorie
  getCategoriaDocumenti(): void {
    this.categoriaDocumentiService.getAllCategoriaDocumenti().subscribe({
      next: (data: CategoriaDocumentiData[]) => {
        this.categoriesValue = data;  // Memorizza il risultato nell'array
        console.log(this.categoriesValue);  // Debug: logga i documenti in console

        // Trasformare i dati in un formato compatibile con p-dropdown
        this.groupedCategorie = this.categoriesValue.map(category => ({
          label: category.nomeCategoria, // Nome del gruppo (categoria)
          value: category.id,            // ID del gruppo (opzionale)
          items: category.sottoCategorie.map(subCategory => ({
              label: subCategory.nomeSottoCategoria, // Nome dell'opzione (sotto-categoria)
              value: subCategory.id                 // ID dell'opzione (sotto-categoria)
          }))
        }));

      },
      error: (err) => {
        console.error('Errore nel recupero delle categorie documenti', err);
      }
    });
  }
  
  // Get by id documento
  getDocumentoById(idDocumento: number): void {
    this.documentiService.getDocumentoById(idDocumento).subscribe({
      next: (data: DocumentiData) => {
        this.documento = data;  // Memorizza il risultato nell'array
        console.log(this.documento);  // Debug: logga i documenti in console

        // Acquisco i dettagli del file
        this.getFileDocumentiById(this.documento.idFiles);

        this.setFormValueDatiGenerali();  // Setto il form con i dati acquisiti

        this.loadingDocumento = false;  // Imposta lo stato di caricamento a false
        this.successLoadingDocumento = true;
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);

        this.loadingDocumento = false;  // Anche in caso di errore, imposta lo stato di caricamento a false
        this.successLoadingDocumento = false;
      }
    });
  }

  // GetFileDocumenti by list id
  getFileDocumentiById(idFiles: number[]) {
    this.fileDocumentiService.getFileDocumentiById(idFiles).subscribe({
      next: (data: FileDocumentiData[]) => {
        this.filesValue = data;  // Memorizza il risultato nell'array
        console.log('this.filesValue -> ', this.filesValue);  // Debug: logga i documenti in console
        this.filesValueBackup = [...data];  // Crea una copia superficiale usando l'operatore spread
        console.log('this.filesValueBackup -> ', this.filesValueBackup);  // Debug: logga i documenti in console

        this.loadingFileDocumento = false;  // Imposta lo stato di caricamento a false
        this.successLoadingFileDocumento = true;
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);

        this.loadingFileDocumento = false;  // Anche in caso di errore, imposta lo stato di caricamento a false
        this.successLoadingFileDocumento = false;
      }
    });
  }

  getFileDocumentoDownloadById(idFile: number) {
    this.fileDocumentiService.getFileDocumentoDownloadById(idFile).subscribe({
      next: (response) => {
        // Estrarre il blob dalla risposta
        const fileBlob: Blob = response.body!;

        // Estrarre il nome del file dall'header Content-Disposition
        const contentDisposition = response.headers.get('Content-Disposition');

        let fileName = 'fileScaricato';  // Valore di default se non troviamo l'header

        // Controlla se l'header Content-Disposition esiste e contiene un nome file
        if (contentDisposition) {
          console.log('Content-Disposition Header:', contentDisposition);  // Controlla il contenuto dell'header

          // Aggiorna la regex per gestire sia 'filename=' che 'filename*='
          const fileNameMatch = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
          if (fileNameMatch && fileNameMatch.length > 1) {
            fileName = fileNameMatch[1].replace(/['"]/g, '');  // Rimuove eventuali apici
          }
        }

        // Creazione di un link per il download del file
        const downloadURL = window.URL.createObjectURL(fileBlob);
        const link = document.createElement('a');
        link.href = downloadURL;
        link.download = fileName;  // Imposta il nome del file scaricato
        link.click();

        // Liberare l'URL creato per il blob
        window.URL.revokeObjectURL(downloadURL);

        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Download del file effettuato con successo!' })
      },
      error: (err) => {
        console.error('Errore nel download del file:', err);

        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di download del file' })
      }
    });  
  }

  getFileDocumentoDownloadByListId() {

    var idsFile: number[] = [];
    this.filesValueBackup.forEach(file => {
      idsFile.push(file.id);
    });

    this.fileDocumentiService.getFileDocumentoDownloadByListId(idsFile).subscribe({
      next: (response) => {
        // Estrarre il blob dalla risposta
        const fileBlob: Blob = response.body!;

        // Estrarre il nome del file dall'header Content-Disposition
        const contentDisposition = response.headers.get('Content-Disposition');

        let fileName = 'filedocumenti.zip';  // Nome di default per il file zip

        // Controlla se l'header Content-Disposition esiste e contiene un nome file
        if (contentDisposition) {
          console.log('Content-Disposition Header:', contentDisposition);  // Controlla il contenuto dell'header

          // Aggiorna la regex per gestire sia 'filename=' che 'filename*='
          const fileNameMatch = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
          if (fileNameMatch && fileNameMatch.length > 1) {
            fileName = fileNameMatch[1].replace(/['"]/g, '');  // Rimuove eventuali apici
          }
        }

        // Creazione di un link per il download del file
        const downloadURL = window.URL.createObjectURL(fileBlob);
        const link = document.createElement('a');
        link.href = downloadURL;
        link.download = fileName;  // Imposta il nome del file scaricato
        link.click();

        // Liberare l'URL creato per il blob
        window.URL.revokeObjectURL(downloadURL);

        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Download dei file effettuato con successo!' })
      },
      error: (err) => {
        console.error('Errore nel download del file:', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di download dei file ' })
      }
    });  
  }
  
  // Modifica documento
  onSubmitDocumento() {
    if (this.idDocumento > 0) {
      console.log('this.filesValueBackup onSubmitDocumento -> ', this.filesValueBackup);  // Debug: logga i documenti in console

      // GetFormValue
      this.getFormValue();
  
      console.log('this.filesValueBackup -> ', this.filesValueBackup);
      // Inizializzo i dati che erano già presenti ** Questo crea che idfiles precedente ha dati inizializati
      this.filesValueBackup.forEach(x => this.updateDocumentiValue.idFiles.push(x.id));
      console.log('onSubmitDocumento -> ', this.updateDocumentiValue);
      
      this.updateDocumento();
    }
    else {
      // GetFormValue
      this.getFormValueCreate();

      console.log('this.createDocumentoValue -> ', this.createDocumentoValue);

      this.createDocumento();
    }

    /*
        Ho this.filesInserimento che contiene i dati di tipo File da inserire come IfromFile sul back-end
        Ho this.updateDocumentiValue che contiene tutti i dati del documento è gli idFiles che erano presenti durante la modifica

        // Chiamata REST
    */
    
    // Modifico rest in modo che:
    // Come parametro ha anche idFiles[] che inserisco prima della chiamata update
    // All'interno vengono inizializzati, prima della chiamata, con i file "originali" rimasti durtante la modifica
    // (modifica: ogni delete del file, verifico se id presente nel array dei file originali, se presente elimino)
    // Modifico procedura rest in modo che, durante la verifica di eliminazione o inserimento file, elimina i file con id diversi da idFiles
    // Ed inserisce i nuovi file passati tramite parametro IFromFile

    // Acqusisire e passare i nuovi dati del documento
    // Acquisire e passare i file da carica

    // this.documentiService.updateDocumento().subscribe({
    //   next: (data: DocumentiData) => {
    //     this.documento = data;  // Memorizza il risultato nell'array
    //     console.log(this.documento);  // Debug: logga i documenti in console
    //   },
    //   error: (err) => {
    //     console.error('Errore nel recupero dei documenti', err);
    //   }
    // });
  }

  onDownload(idFile: number) {
    this.getFileDocumentoDownloadById(idFile);
  }

  onOpenDialogDelete(event: Event) {

    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Vuoi eliminare questo documento definitivamente?',
      header: 'Conferma cancellazione documento',
      icon: 'fa-solid fa-circle-exclamation',
      acceptButtonStyleClass:"p-button-danger p-button-text",
      rejectButtonStyleClass:"p-button-text p-button-text",
      acceptIcon:"none",
      rejectIcon:"none",

      accept: () => {
        this.idDocumentoDelete = this.idDocumento;
        this.onDeleteDocument();
      },
      reject: () => {
        this.messageService.add({ severity: 'info', summary: 'Indietro', detail: 'Annullata la procedura di eliminazione del documento' });
      }
    });

  }

  // onCloseDialogDelete() {
  //   this.visibleDelete = false;
  //   this.idDocumentoDelete = 0;
  // }

  onOpenDialogDeleteFile(event: Event, idFile: number) {
    // this.visibleDeleteFile = true;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Vuoi eliminare questo file?',
      header: 'Conferma cancellazione file',
      icon: 'fa-solid fa-circle-exclamation',
      acceptButtonStyleClass:"p-button-danger p-button-text",
      rejectButtonStyleClass:"p-button-text p-button-text",
      acceptIcon:"none",
      rejectIcon:"none",

      accept: () => {
        this.idFileDelete = idFile;
        this.onDeleteFile();
      },
      reject: () => {
        this.messageService.add({ severity: 'info', summary: 'Indietro', detail: 'Annullata la procedura di eliminazione del file appartenente al documento' });
      }
    });
  }

  onCloseDialogDeleteFile() {
    // this.visibleDeleteFile = false;
    this.idFileDelete = 0;
  }

  updateDocumento() {
    this.documentiService.updateDocumento(this.updateDocumentiValue, this.filesInserimento).subscribe({
      next: (data: DocumentiData) => {
        this.documento = data;  // Memorizza il risultato nell'array
        console.log('updateDocumento -> ', this.documento);  // Debug: logga i documenti in console
        this.location.back();
        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Modifica del documento effettuata con successo!' })
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di modifica del documento' })
      }
    });
  }

  createDocumento() {
    this.documentiService.CreateDocumento(this.createDocumentoValue, this.filesInserimento).subscribe({
      next: (data: DocumentiData) => {
        this.documento = data;  // Memorizza il risultato nell'array
        console.log('createDocumento -> ', this.documento);  // Debug: logga i documenti in console
        this.location.back();
        this.messageService.clear();
        this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Creazione del documento effettuata con successo!' })
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di creazione del documento' })
      }
    });
  }

  onDeleteDocument() {
    this.documentiService.DeleteDocumento(this.idDocumento).subscribe({
      next: (result: boolean) => {
        this.resultDeleteDocumento = result;  // Memorizza il risultato nell'array
        console.log('this.resultDeleteDocumento -> ', this.resultDeleteDocumento);  // Debug: logga i documenti in console
        if (this.resultDeleteDocumento) {
          // this.visibleDelete = false;
          this.location.back();
          this.messageService.clear();
          this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Eliminazione del documento effettuato con successo!' })
        }
        else {
          this.messageService.clear();
          this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di eliminazione del documento' })
        }
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);
        this.messageService.clear();
        this.messageService.add({ severity: 'error', summary: 'Errore', detail: 'Errore durante la procedura di eliminazione del documento' })
      }
    });
  }

  onDeleteFile() {
    // Array di backup, elimino il file ricevuto
    this.filesValueBackup = this.filesValueBackup.filter(file => file.id !== this.idFileDelete);

    // Trovo il nome ed estensione del file per eliminarlo nell'array dei File da inviare alla REST
    let fileToDelete = this.uploadedFiles.find(file => file.id === this.idFileDelete);
    if (fileToDelete) {
      // Se il file è stato trovato, memorizza il nome e l'estensione in una variabile
      let nameUploadFileDelete = `${fileToDelete.nomeFile}.${fileToDelete.estensioneFile}`;
      // Array di tipo file, elimino il file momentaneo
      this.filesInserimento = this.filesInserimento.filter(file => file.name !== nameUploadFileDelete);
    }

    // Array dei dati inseriti momentaneamente, elimino il file ricevuto
    this.uploadedFiles = this.uploadedFiles.filter(file => file.id !== this.idFileDelete);

    // Array di visualizzazione, elimino il file ricevuto
    this.filesValue = this.filesValue.filter(file => file.id !== this.idFileDelete);

    // this.visibleDeleteFile = false;

    this.messageService.clear();
    this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Eliminazione del file effettuata con successo!' })
  }

  // Gestisce la selezione dei file
  onFileSelect(event: any) {
    console.log('Event:', event);  // Check what the event object contains
    const timestamp = new Date().getTime();

    // Cicla attraverso i file selezionati
    for (let file of event.files) {

      this.filesInserimento.push(file);

      const uniqueId = timestamp + Math.floor(Math.random() * 1000);  // Genera un ID unico
      this.idUnique.push(uniqueId);

      // Crea un oggetto FileDocumentiData per ogni file selezionato
      const fileData: FileDocumentiDataValue = {
        id: uniqueId,  // L'ID può essere generato dinamicamente o dal backend
        nomeFile: this.getFileNameWithoutExtension(file.name),  // Nome del file
        estensioneFile: this.getFileExtension(file.name),  // Estensione del file
        dataInserimentoFile: new Date(),  // Data di inserimento attuale
        documentiId: this.idDocumento  // Popola l'ID del documento se disponibile
      };

      // Aggiunge il file all'array
      this.uploadedFiles.push(fileData);

      const fileDataVal: FileDocumentiData = {
        id: uniqueId,  // L'ID può essere generato dinamicamente o dal backend
        nomeFile: this.getFileNameWithoutExtension(file.name),  // Nome del file
        estensioneFile: this.getFileExtension(file.name),  // Estensione del file
        percorsoFile: '',
        dataInserimentoFile: new Date(),  // Data di inserimento attuale
        documentiId: this.idDocumento,  // Popola l'ID del documento se disponibile
        isTemporary: true
      };

      // Aggiungo dei file visualizzazione
      this.filesValue.push(fileDataVal);
    }

    // Forza l'aggiornamento della tabella facendo una copia dell'array
    this.filesValue = [...this.filesValue];

    console.log('Array fileDocumentiData:', this.uploadedFiles);

    console.log('this.filesValueBackup -> ', this.filesValueBackup);  // Debug: logga i documenti in console

    this.messageService.clear();
    this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'File inserito con successo!' })

    this.fileUploader.clear();
  }

  // Metodo per ottenere l'estensione del file
  getFileExtension(filename: string): string {
    const ext = filename.split('.').pop() || '';
    return ext ? `.${ext}` : '';  // Aggiunge il punto se esiste un'estensione
  }

  getFileNameWithoutExtension(filename: string): string {
    return filename.split('.').slice(0, -1).join('.') || filename;
  }

  private setFormDatiGeneraliUpdate(): FormGroup {
    return this.fb.group({
      Titolo: new FormControl({ value: '', disabled: !this.checked }, Validators.required),
      Descrizione: new FormControl({ value: '', disabled: !this.checked }, Validators.required),
      DataCreazioneDocumento: new FormControl({ value: '', disabled: !this.checked }, Validators.required),
      DataInserimentoDocumento: new FormControl({ value: '', disabled: !this.checked }, Validators.required),
      SottoCategoriaDocumentiId: new FormControl({ value: null, disabled: !this.checked }, Validators.required),
      // Titolo: [{ value: null, disabled: !this.checked }],
      // Descrizione: [{ value: null, disabled: !this.checked }],
      // DataCreazioneDocumento: [{ value: null, disabled: !this.checked }],
      // DataInserimentoDocumento: [{ value: null, disabled: true }],
      // CategoriaDocumentiId: [{ value: null, disabled: !this.checked }],
    });
  }

  private setFormDatiGeneraliCreate(): FormGroup {
    return this.fb.group({
      Titolo: new FormControl('', Validators.required),
      Descrizione: new FormControl('', Validators.required),
      DataCreazioneDocumento: new FormControl('', Validators.required),
      DataInserimentoDocumento: new FormControl({ value: new Date(), disabled: true }),
      SottoCategoriaDocumentiId: new FormControl(null, Validators.required),
      // Titolo: [null],
      // Descrizione: [null],
      // DataCreazioneDocumento: [null],
      // DataInserimentoDocumento: [{ value: new Date(), disabled: true }],
      // CategoriaDocumentiId: [null],
    });
  }

  private setFormValueDatiGenerali(): void {
    this.formDocumento.setValue({
      Titolo: this.documento.titolo,
      Descrizione: this.documento.descrizione,
      DataCreazioneDocumento: new Date(this.documento.dataCreazioneDocumento),
      DataInserimentoDocumento: new Date(this.documento.dataInserimentoDocumento),
      SottoCategoriaDocumentiId: this.documento.sottoCategoriaDocumentiId,
    })
  }

  private getFormValue() {

    this.updateDocumentiValue = {
      id: this.idDocumento,
      titolo: this.formDocumento.get("Titolo")?.value,
      descrizione: this.formDocumento.get("Descrizione")?.value,
      dataCreazioneDocumento: this.formDocumento.get("DataCreazioneDocumento")?.value,
      dataInserimentoDocumento: new Date(this.documento.dataInserimentoDocumento),
      sottoCategoriaDocumentiId: this.formDocumento.get("SottoCategoriaDocumentiId")?.value,
      idFiles: []
    }

    console.log('getFormValue -> ', this.updateDocumentiValue);
  }

  private getFormValueCreate() {

    this.createDocumentoValue = {
      Titolo: this.formDocumento.get("Titolo")?.value,
      Descrizione: this.formDocumento.get("Descrizione")?.value,
      DataCreazioneDocumento: this.formDocumento.get("DataCreazioneDocumento")?.value,
      SottoCategoriaDocumentiId: this.formDocumento.get("SottoCategoriaDocumentiId")?.value,
    }

    console.log('getFormValueCreate -> ', this.updateDocumentiValue);
  }

}

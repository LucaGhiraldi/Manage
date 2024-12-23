import { Component, OnInit } from '@angular/core';

import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { MessagesModule } from 'primeng/messages';

import { DocumentiData, DocumentiFilter } from '../../../M-Models/Documenti';
import { DocumentiService } from '../../../M-Services/API/Documenti/documenti.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CategoriaDocumentiData } from '../../../M-Models/CategoriaDocumenti';
import { CategoriaDocumentiService } from '../../../M-Services';
import { Router } from '@angular/router';

import { saveAs } from 'file-saver-es';
import { InputTextModule } from 'primeng/inputtext';
import { SelectItemGroup } from 'primeng/api';


@Component({
  selector: 'app-documenti',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, ReactiveFormsModule, PanelModule, TableModule, FloatLabelModule, CalendarModule, DropdownModule, ButtonModule, MessagesModule, InputTextModule ],
  providers: [DocumentiService, CategoriaDocumentiService],  // Provide DocumentiService
  templateUrl: './documenti.component.html',
  styleUrl: './documenti.component.scss'
})
export class DocumentiComponent implements OnInit {
  
  documentiFilter: DocumentiFilter = {};
  documenti: DocumentiData[] = [];  // Variabile per memorizzare i documenti
  idUtente: number = 1; // ID utente da passare, ad esempio 1
  loading: boolean = true;  // Stato del caricamento
  successLoading = false;   // Risultato del caricamento

  formDocumentSearch!: FormGroup;
  categoriesValue: CategoriaDocumentiData[] = [];
  groupedCategorie: SelectItemGroup[] = [];
  // selectedSottoCategoria: CategoriaDocumentiData | null = null;
  // selectedCategoria: CategoriaDocumentiData | null = null;
  
  doReset: boolean = false;
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
      detail: "Problemi nell'acquisizione dei dati del documento...", 
    }, 
  ]; 

  constructor(private documentiService: DocumentiService,
              private categoriaDocumentiService: CategoriaDocumentiService,
              private fb: FormBuilder,
              private router: Router) {}

  ngOnInit(): void {

    this.doReset = false;

    this.formDocumentSearch = this.setForm();

    console.log(this.documentiFilter);

    this.getCategoriaDocumenti();

    // this.prepareGroupedCategories(this.categoriesValue);

    this.getDocumenti();
  }

  onSubmitDocumentSearch() {

    this.doReset = true;

    this.getFormValue();

    this.getDocumenti();
  }

  clearForm() {
    this.formDocumentSearch.reset();

    if (this.doReset) {
      this.getFormValue();

      this.getDocumenti();
    }

  }

  onGoToCategoria() {
    this.router.navigate(['Categoria']);  
  }

  onGoToDocumento(idDocumento: number) {
    this.router.navigate(['Documenti', idDocumento]);  
  }

  onGoToNewDocumento() {
    this.router.navigate(['Documenti', 0]);  
  }

  onExportExcel() {
    import('xlsx').then(xlsx => {
      // Mappa i dati per creare un array di oggetti semplici
      const exportData = this.documenti.map(doc => {
        return {
          // Id: doc.id,
          Titolo: doc.titolo,
          Descrizione: doc.descrizione,
          DataCreazioneDocumento: doc.dataCreazioneDocumento,
          DataInserimentoDocumento: doc.dataInserimentoDocumento,
          UtenteId: doc.utenteId,
          CategoriaDocumentiId: doc.categoriaDocumentiId,
          SottoCategoriaDocumentiId: doc.sottoCategoriaDocumentiId,
          NomeCategoriaDocumenti: doc.nomeCategoriaDocumenti,
          IdFiles: doc.idFiles.join(', '),  // Unisci gli id dei file in una stringa
          NomiFiles: doc.nomiFiles.join(', ')  // Unisci i nomi dei file in una stringa
        };
      });

      const worksheet = xlsx.utils.json_to_sheet(exportData); // Crea il foglio
      const workbook = { Sheets: { 'Informazioni documenti': worksheet }, SheetNames: ['Informazioni documenti'] }; // Crea il workbook
      const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' }); // Scrivi il buffer

      this.saveAsExcelFile(excelBuffer, 'documenti');
    });
  }

  // Metodo per salvare il file Excel
  saveAsExcelFile(buffer: any, fileName: string): void {
    const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const EXCEL_EXTENSION = '.xlsx';
    const data: Blob = new Blob([buffer], { type: EXCEL_TYPE });

    const today = new Date();
    const day = String(today.getDate()).padStart(2, '0');
    const month = String(today.getMonth() + 1).padStart(2, '0'); // Mesi da 0 a 11
    const year = today.getFullYear();

    const formattedDate = `${day}-${month}-${year}`;
    const fileNameWithDate = `${fileName}_export_${formattedDate}${EXCEL_EXTENSION}`;

    saveAs(data, fileNameWithDate);
  }

  getDocumenti(): void {

    console.log("getDocumenti -> " + this.documentiFilter);

    this.documentiService.getAllDocumenti(this.documentiFilter).subscribe({
      next: (data: DocumentiData[]) => {
        this.documenti = data;  // Memorizza il risultato nell'array
        console.log(this.documenti);  // Debug: logga i documenti in console
        this.loading = false;  // Imposta lo stato di caricamento a false
        this.successLoading = true;
      },
      error: (err) => {
        console.error('Errore nel recupero dei documenti', err);
        this.loading = false;  // Anche in caso di errore, imposta lo stato di caricamento a false
        this.successLoading = false;
      }
    });
  }

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

  // prepareGroupedCategories(categories: CategoriaDocumentiData[]): void {
  //   this.groupedCategorie = categories.map(categoria => {
  //     return {
  //         label: categoria.nomeCategoria,   // Nome della categoria come etichetta
  //         value: categoria.id,              // ID della categoria come valore
  //         items: categoria.sottoCategorie.map(sottoCategoria => ({
  //             label: sottoCategoria.nomeSottoCategoria,  // Nome della sotto-categoria come etichetta
  //             value: sottoCategoria.id                   // ID della sotto-categoria come valore
  //         }))
  //     };
  //   });
  // }

  // Metodo che si attiva quando cambia la selezione
  // onSelectionChange() {
  //   // Trova la categoria corrispondente alla sotto-categoria selezionata
  //   if (this.selectedSottoCategoria) {
  //       this.selectedCategoria = this.categoriesValue.find(categoria =>
  //           categoria.sottoCategorie.some(sottoCategoria => sottoCategoria.id === this.selectedSottoCategoria?.id)
  //       ) || null;
  //   } else {
  //       this.selectedCategoria = null;
  //   }
  // }

  // Funzione per trasformare i dati
  // prepareGroupedCategories(categories: CategoriaDocumentiData[]): void {
  //   this.groupedCategories = categories.map(categoria => ({
  //     label: categoria.nomeCategoria, // Nome del gruppo (categoria)
  //     value: categoria.id,
  //     items: categoria.sottoCategorie.map(sottoCategoria => ({
  //       label: sottoCategoria.nomeSottoCategoria, // Etichetta visibile nella dropdown
  //       value: sottoCategoria.id, // Valore selezionabile
  //       categoryId: categoria.id,
  //       categoryLabel: categoria.nomeCategoria
  //     }))
  //   }));
  // }

  private setForm(): FormGroup {
    return this.fb.group({
      Titolo: [null],
      Descrizione: [null],
      DataCreazioneDocumento: [null],
      DataInserimentoDocumento: [null],
      SottoCategoriaDocumentiId: [null],
      // idFiles: [null]
    });
  }
  
  private getFormValue() {

    this.documentiFilter.Titolo = this.formDocumentSearch.get("Titolo")?.value;
    this.documentiFilter.Descrizione = this.formDocumentSearch.get("Descrizione")?.value;
    this.documentiFilter.DataCreazioneDocumento = this.formDocumentSearch.get("DataCreazioneDocumento")?.value;
    this.documentiFilter.DataInserimentoDocumento = this.formDocumentSearch.get("DataInserimentoDocumento")?.value;
    
    this.documentiFilter.SottoCategoriaId = this.formDocumentSearch.get("SottoCategoriaDocumentiId")?.value;

    // this.documentiFilter.CategoriaDocumentiId = this.formDocumentSearch.get("CategoriaDocumentiId")?.value;
    
    if (this.documentiFilter.DataCreazioneDocumento !== undefined && 
      this.documentiFilter.DataCreazioneDocumento !== null      && 
      this.documentiFilter.DataCreazioneDocumento.toString() !== "") 
    {
      var date = new Date(this.documentiFilter.DataCreazioneDocumento);
      this.documentiFilter.DataCreazioneDocumento = date
    }

    if (this.documentiFilter.DataInserimentoDocumento !== undefined && 
      this.documentiFilter.DataInserimentoDocumento !== null      && 
      this.documentiFilter.DataInserimentoDocumento.toString() !== "") 
    {
      var date = new Date(this.documentiFilter.DataInserimentoDocumento);
      this.documentiFilter.DataInserimentoDocumento = date
    }

    if (this.documentiFilter.SottoCategoriaId !== undefined &&
      this.documentiFilter.SottoCategoriaId !== null      && 
      this.documentiFilter.SottoCategoriaId.toString() !== "") {
        this.documentiFilter.SottoCategoriaId = this.documentiFilter.SottoCategoriaId;
    }
    else {
      this.documentiFilter.SottoCategoriaId = 0;
    }

    console.log(this.documentiFilter);
  }

}

import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DocumentiCreateData, DocumentiData, DocumentiDataValue, DocumentiFilter } from '../../../M-Models/Documenti';

@Injectable({
  providedIn: 'root'
})
export class DocumentiService {

  private apiUrl = 'https://localhost:44390/api/Documenti';

  constructor(private http: HttpClient) { }

  getAllDocumenti(documentiFilter: DocumentiFilter): Observable<DocumentiData[]> {
    return this.http.post<DocumentiData[]>(`${this.apiUrl}/GetAll`, documentiFilter, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'  // Assicurati che Content-Type sia 'application/json'
      })
    });
  }

  getDocumentoById(idDocumento: number): Observable<DocumentiData> {
    return this.http.get<DocumentiData>(`${this.apiUrl}/GetById/${idDocumento}`);
  }

  // Update documento e files
  updateDocumento(updateDocumentoValue: DocumentiDataValue, files: File[]): Observable<DocumentiData> {

    const formData: FormData = new FormData();
    files.forEach(file => {
      formData.append('files', file);
    });

    // Aggiungi ogni campo dell'oggetto Documento come campo separato di FormData
    formData.append('Id', updateDocumentoValue.id.toString());
    formData.append('Titolo', updateDocumentoValue.titolo);
    formData.append('Descrizione', updateDocumentoValue.descrizione);
    formData.append('DataCreazioneDocumento', updateDocumentoValue.dataCreazioneDocumento.toISOString());
    formData.append('SottoCategoriaDocumentiId', updateDocumentoValue.sottoCategoriaDocumentiId.toString());

    // Aggiungi l'array di idFiles se presente e non vuoto, altrimenti aggiungi un array vuoto
    if (updateDocumentoValue.idFiles && updateDocumentoValue.idFiles.length > 0) {
      updateDocumentoValue.idFiles.forEach((idFile, index) => {
            formData.append(`idFiles[${index}]`, idFile.toString());
      });
    } 

    return this.http.put<DocumentiData>(`${this.apiUrl}/Update`, formData);      
  }

  CreateDocumento(documentoCreate: DocumentiCreateData, files: File[]): Observable<DocumentiData> {
    const formData: FormData = new FormData();
    files.forEach(file => {
      formData.append('files', file);
    });

    // Aggiungi ogni campo dell'oggetto Documento come campo separato di FormData
    formData.append('Titolo', documentoCreate.Titolo);
    formData.append('Descrizione', documentoCreate.Descrizione);
    formData.append('DataCreazioneDocumento', documentoCreate.DataCreazioneDocumento.toISOString());
    formData.append('SottoCategoriaDocumentiId', documentoCreate.SottoCategoriaDocumentiId.toString());

    return this.http.post<DocumentiData>(`${this.apiUrl}/Create`, formData);     
  }

  DeleteDocumento(idDocumento: number): Observable<boolean> {
    const options = {
      body: idDocumento  // Imposta il body con l'ID del documento
    };
  
    return this.http.delete<boolean>(`${this.apiUrl}/Delete`, options);

  }
        
}

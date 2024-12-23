import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FileDocumentiData } from '../../../M-Models/FileDocumenti';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileDocumentiService {

  private apiUrl = 'https://localhost:44390/api/FileDocumenti';

  constructor(private http: HttpClient) { }

  // Metodo per scaricare il file, osservando l'intera risposta per accedere agli headers
  getFileDocumentoDownloadById(idFile: number): Observable<HttpResponse<Blob>> {
    return this.http.get(`${this.apiUrl}/Download/${idFile}`, { 
      responseType: 'blob',  // Riceviamo il file come blob
      observe: 'response'    // Osserviamo l'intera risposta per accedere agli headers
    });
  }

    // Metodo per scaricare il file, osservando l'intera risposta per accedere agli headers
    getFileDocumentoDownloadByListId(ids: number[]): Observable<HttpResponse<Blob>> {
      // Costruisci la stringa della query con gli ID
      const idParams = ids.map(id => `ids=${id}`).join('&');

      return this.http.get(`${this.apiUrl}/DownloadAll?${idParams}`, {
        responseType: 'blob',  // Riceviamo il file come blob
        observe: 'response'    // Osserviamo l'intera risposta per accedere agli headers
      });
    }
  
  getFileDocumentiById(idFiles: number[]): Observable<FileDocumentiData[]> {
    return this.http.post<FileDocumentiData[]>(`${this.apiUrl}/GetByListId`, idFiles);
  }

}

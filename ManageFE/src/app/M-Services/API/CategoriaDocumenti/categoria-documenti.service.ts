import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoriaDocumentiCreateData, CategoriaDocumentiData, CategoriaDocumentiUpdateData } from '../../../M-Models/CategoriaDocumenti';

@Injectable({
  providedIn: 'root'
})
export class CategoriaDocumentiService {

  private apiUrl = 'https://localhost:44390/api/CategoriaDocumenti';

  constructor(private http: HttpClient) { }

  getAllCategoriaDocumenti(): Observable<CategoriaDocumentiData[]> {
    return this.http.get<CategoriaDocumentiData[]>(`${this.apiUrl}/GetAll`);  
  }

  // Create
  CreateCategory(categoryCreate: CategoriaDocumentiCreateData): Observable<CategoriaDocumentiData> {
    return this.http.post<CategoriaDocumentiData>(`${this.apiUrl}/Create`, categoryCreate);
  }

  // Modify
  UpdateCategory(categoryUpdate: CategoriaDocumentiUpdateData): Observable<CategoriaDocumentiData> {
    return this.http.put<CategoriaDocumentiData>(`${this.apiUrl}/Update`, categoryUpdate);
  } 

  // Delete
  DeleteCategory(idCategory: number): Observable<boolean> {
    const options = {
      body: idCategory  // Imposta il body con l'ID del documento
    };
  
    return this.http.delete<boolean>(`${this.apiUrl}/Delete`, options);
  }
}

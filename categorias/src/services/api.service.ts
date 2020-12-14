import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Categoria } from 'src/model/categoria';
import { Usuario } from 'src/model/usuario';

const apiUrl = 'https://localhost:5001/api/categorias';
const apiLoginUrl = 'https://localhost:5001/api/autoriza/login';
var token = '';
var httpOptions = { headers: new HttpHeaders({ "Content-Type": "application/json" }) }

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  montaHeaderToken() {
    token = localStorage.getItem("jwt");
    console.log('jwt header token ' + token);
    httpOptions = { headers: new HttpHeaders({ "Authorization": "Bearer " + token, "Content-Type": "application/json" }) };
  }

  login(Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(apiLoginUrl, Usuario).pipe(
      tap((Usuario: Usuario) => console.log(`Login usuario com email=${Usuario.email}`)),
      catchError(this.handleError<Usuario>('Login'))
    );
  }

  getCategorias(): Observable<any> {
    this.montaHeaderToken();
    console.log(httpOptions.headers);
    return this.http.get<any>(apiUrl, httpOptions)
      .pipe(
        tap(Categorias => console.log('leu as Categorias')),
        catchError(this.handleError('getCategorias', []))
      );
  }

  getCategoria(id: number): Observable<Categoria> {
    return this.http.get<Categoria>(`${apiUrl}/${id}`, httpOptions)
      .pipe(
        tap(Categorias => console.log(`leu as Categoria id=${id}`)),
        catchError(this.handleError<Categoria>(`getCategoria id=${id}`))
      );
  }

  addCategoria(Categoria): Observable<Categoria> {
    this.montaHeaderToken();
    return this.http.post<Categoria>(apiUrl, Categoria, httpOptions).pipe(
      tap((Categoria: Categoria) => console.log(`adicionou a Categoria com id=${Categoria.categoriaId}`)),
      catchError(this.handleError<Categoria>('addCategoria'))
    );
  }

  updateCategoria(id, Categoria): Observable<any> {
    return this.http.put(`${apiUrl}/${id}`, Categoria, httpOptions).pipe(
      tap(_ => console.log(`atualiza a categoria com id=${id}`)),
      catchError(this.handleError<any>('updateCategoria'))
    );
  }

  deleteCategoria(id: number): Observable<Categoria> {
    return this.http.delete<Categoria>(`${apiUrl}/${id}`, httpOptions).pipe(
      tap(_ => console.log(`remove a Categoria com id=${id}`)),
      catchError(this.handleError<Categoria>('deleteCategoria'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.log(error);
      return of(result as T);
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Categoria } from 'src/model/categoria';
import { ApiService } from 'src/services/api.service';

@Component({
  selector: 'app-categoria-nova',
  templateUrl: './categoria-nova.component.html',
  styleUrls: ['./categoria-nova.component.scss']
})
export class CategoriaNovaComponent implements OnInit {
  categoriaForm; FormGroup;
  nome: string = '';
  imagemUrl: string = '';
  dataSource: Categoria;
  isLoadingResults = false;

  constructor(
    private router: Router,
    private api: ApiService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.categoriaForm = this.formBuilder.group({
      'nome': [null, Validators.required],
      'imagemUrl': [null, Validators.required]
    });
  }

  addCategoria(form: NgForm) {
    this.isLoadingResults = true;
    this.api.addCategoria(form)
      .subscribe(res => {
        this.dataSource = res;
        this.router.navigate(['/categorias']);
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }

}

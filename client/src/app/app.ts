import { Component, inject, OnInit } from '@angular/core';
import { Header } from "./layout/header/header";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  title = 'client';
  private http = inject(HttpClient);
  protected products: Product[] = [];

  ngOnInit(): void {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: products => {
        console.log(products);
        this.products = products.data;
      },
      error: error => {
        console.error('There was an error!', error);
      },
      complete: () => {
        console.log('Request completed');
      }
    });
  }
}

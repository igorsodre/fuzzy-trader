import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class InvestmentService {
  private readonly urlPrefix = environment.baseUrl + '/api/investment';
  constructor(private http: HttpClient) {}

  getInvestmentOptions() {
    const endpoint = this.urlPrefix + '/get-investment-options';
    return this.http.get<string>(endpoint);
  }
}

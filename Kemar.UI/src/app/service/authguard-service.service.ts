import { Injectable } from '@angular/core';
import { TokenStroageService } from './token-stroage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthguardServiceService {

  constructor(private tokenStorage: TokenStroageService) { }

    gettoken(){  
    return !!this.tokenStorage.getToken();  
    }
}

import $api from "../http";
import {AxiosResponse} from 'axios';
import { AuthResponse } from "../models/AuthResponse";


export default class AuthService {
  
    static async login(username: string, password: string) : Promise<AxiosResponse<AuthResponse>> {
        return $api.post('api/authorization/signin/client', {username, password})
    }

    static async registration(username: string, password: string) : Promise<AxiosResponse<AuthResponse>> {
        return $api.post('/api/client/signup', {username, password})
    }

    static async logout() {
        return $api.post('api/authorization/client/logout')
    }
  //  static async logout(username: string, password: string) : Promise<void> {
  //      return $api.post('/api/client/signup', {username, password})
  //  }

}


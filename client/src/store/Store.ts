import { IUser } from "../models/IUser";
import { makeAutoObservable } from "mobx";
import AuthService from "../services/AuthService";
import axios from "axios";
import { AuthResponse } from "../models/AuthResponse";
import { API_URL } from "../http";

export default class Store {
    user = {} as IUser;
    isAuth = false;
    isLoading = false;

    constructor() {
        makeAutoObservable(this);
    }

    SetAuth(bool: boolean) {
        this.isAuth = bool; 
    }

    SetUser(id: string, username: string) {
        this.user.id = id;
        this.user.username = username;
        console.log("User set:");
        console.log(this.user.id);
    }

    SetLoading(bool: boolean) {
        this.isLoading = bool;
    }

    async login(username: string, password: string) {
        try {
            const response = await AuthService.login(username, password);
            console.log(response);
            localStorage.setItem('token', response.data.token);
            this.SetUser(response.data.id, response.data.username);
            this.SetAuth(true);
        } catch (e) {
            console.log(e);
        }
    }

    async registration(username: string, password: string) {
        try {
            const response = await AuthService.registration(username, password);
            console.log(response);
            localStorage.setItem('token', response.data.token);
            this.SetUser(response.data.id, response.data.username);
            this.SetAuth(true);
        } catch (e) {
            console.log(e);
        }
    }

    async logout() {
        try {
            const response = await AuthService.logout();
            console.log(response);
            localStorage.removeItem('token');
            this.SetAuth(false);
            this.SetUser({} as string, {} as string);
        } catch(e) {
            console.log(e);
        }
        
    }

    async checkAuth() {
        this.SetLoading(true);
        try {
            const response = await axios.get<AuthResponse>(`${API_URL}/api/authorization/client/refresh`, {withCredentials: true});
            console.log(response);
            localStorage.setItem(`token`, response.data.token);
            this.SetAuth(true);
            this.SetUser(response.data.id, response.data.username);
        } catch (e) {
            console.log(e);
        }
        this.SetLoading(false);
    }

}
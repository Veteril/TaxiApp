import { IUser } from "../models/IUser";
import { action, makeAutoObservable, runInAction } from "mobx";
import AuthService from "../services/AuthService";
import axios from "axios";
import { AuthResponse } from "../models/AuthResponse";
import { API_URL } from "../http";
import OrderService from "../services/OrderService";
import { OrderCreate } from "../models/OrderCreate";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { API_URL_ORDER } from "../http/orders";
import { useState } from "react";
import { OrderRead } from "../models/OrderRead";
import { stat } from "fs";


export default class Store {
    
    connection = null as HubConnection | null;
    messages: { user: string; message: string; }[] = [];
    user = {} as IUser;
    isAuth = false;
    isLoading = false;
    orderId = {} as string;
    isOrder = false;
    order = {} as OrderRead;
    isAccepted = false;

    constructor() {
        makeAutoObservable(this);
    }

    SetAuth(bool: boolean) {
        this.isAuth = bool; 
    }

    SerIsAccepted(bool: boolean) {
        this.isAccepted = bool;
    }

    SetIsOrder(bool: boolean) {
        this.isOrder = bool;
    }

    SetUser(id: string, username: string, role: string) {
        this.user.id = id;
        this.user.username = username;
        this.user.role = role;
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
            this.SetUser(response.data.id, response.data.username, response.data.role);
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
            this.SetUser(response.data.id, response.data.username, response.data.role);
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
            this.SetUser({} as string, {} as string, {} as string);
        } catch(e) {
            console.log(e);
        }
        
    }

    async createOrder(status: number, destination: string, address: string) {
        try {
          // console.log(order);
            const response = await OrderService.createOrder(status, destination, address);
            console.log(response);
            this.order.id = response.data.id;
            this.joinRoom(this.user.username, this.order.id);
        } catch(e) {
            console.log(e);
        }
    }

    async checkAuth() {
        this.SetLoading(true);
        try {
            const response = await axios.get<AuthResponse>(`${API_URL}/api/authorization/refresh`, {withCredentials: true});
            console.log(response);
            localStorage.setItem(`token`, response.data.token);
            this.SetAuth(true);
            this.SetUser(response.data.id, response.data.username, response.data.role);
        } catch (e) {
            console.log(e);
        }
        this.SetLoading(false);
    }

     async joinRoom(username: string, room: string) {
        try {
          this.connection = new HubConnectionBuilder()
          .withUrl(`${API_URL_ORDER}/api/order/chat`)
          .configureLogging(LogLevel.Information)
          .build();
    
          this.connection.on("ReceiveMessage", (user: string, message: string) => {
          //setMessages(messages => [...messages, {user,message}]);
            runInAction(() => 
            {
                this.messages = [...this.messages, {user,message}];
            })
          });
    
          this.connection.onclose(e => {
            runInAction(() => 
            {
                this.connection = null;
                this.messages = [];
            })
          });
    
          await this.connection.start();
          await this.connection.invoke("JoinRoom", username, room);

          //runInAction(() => {
            //this.connection = connection;
          //});
        } catch(e) {
          console.log(e);
        }
    }


    async joinRoomDriver(username: string, room: string) {
        try {
          this.connection = new HubConnectionBuilder()
          .withUrl(`${API_URL_ORDER}/api/order/chat`)
          .configureLogging(LogLevel.Information)
          .build();
    
          this.connection.on("ReceiveMessage", (user: string, message: string) => {
          //setMessages(messages => [...messages, {user,message}]);
            runInAction(() => 
            {
                this.messages = [...this.messages, {user,message}];
            })
          });

          this.connection.on("GetDriverNotification", (order: OrderRead ) => {
            runInAction(() => {
                this.order = order;
                this.isOrder = true;
            }) 
          });
    
          this.connection.onclose(e => {
            runInAction(() => 
            {
                this.connection = null;
                this.messages = [];
            })
          });
    
          await this.connection.start();
          await this.connection.invoke("JoinRoom", username, room);

         // runInAction(() => {
           // this.connection = connection;
        //  });
          
        } catch(e) {
          console.log(e);
        }
    }
}
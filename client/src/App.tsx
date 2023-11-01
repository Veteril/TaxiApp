import React, { useContext, useEffect, useState } from 'react';
import LoginForm from './components/LoginForm';
import { Context } from './index';
import { observer } from 'mobx-react-lite';
import Lobby from './components/Lobby';
import {Button} from "react-bootstrap"
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { API_URL } from './http';
import Chat from './components/Chat';
import './App.css';
import { API_URL_ORDER } from './http/orders';
import { OrderCreate } from './models/OrderCreate';
import { isAccessor } from 'typescript';

function App() {

  const {store} = useContext(Context);
  const [authChecked, setAuthChecked] = useState(false);
  const [Destination, setDestination] = useState<string>('');
  const [Address, setAddress] = useState<string>('');

  // const signalRService = new SignalRService();

  useEffect(() => {
    if (!authChecked && localStorage.getItem('token')) {
      store.checkAuth();
      setAuthChecked(true);
    }
  }, [authChecked, store]);

  const closeConnection = async () => {
    try {
      await store.connection?.stop();
      store.SerIsAccepted(false);
    } catch(e) {
      console.log(e);
    }
  }

  const sendMessage = async (message: string) => {
    try {
      await store.connection?.invoke("SendMessage", message, store.user.username, store.order.id);
    } catch(e) {
      console.log(e);
    }
  }
  
  const acceptOrder = async () => {
    try {
      await store.connection?.invoke("JoinRoom", store.user.username, store.order.id);
      store.SerIsAccepted(true);
    } catch(e) {
      console.log(e);
    }
  }

  if(store.isLoading) {
    return(
        <div>
            <h2>Wait a second...</h2>
        </div>
    );
  }


  if(!store.isAuth) {
    return (
      <div className="App">
        <LoginForm/>
      </div>
    );
  }

  if(store.connection && store.user.role == "Client") {
    return(
      <div className="App">
        <Chat messages={store.messages} sendMessage={sendMessage} closeConnection={closeConnection} />
      </div>
    );
  }

  if(!(store.connection) && store.user.role == "Driver") {
    return (
      <div className="App">
        <Button className='log-out' onClick={() => store.joinRoomDriver(store.user.username, 'drivers')}>
            Start Work
        </Button>
      </div>
    );
  }
  
  if(store.connection && store.user.role == "Driver" && store.isAccepted ==false) {
    return !(store.isOrder) ? (
      <div className="App">Y
        Waiting for Orders
      </div>
    ) : 
    <div className='App'>
        Address :
        Destination :
        <Button onClick={acceptOrder}>
            Accept
        </Button>
        <Button onClick={() => store.SetIsOrder(false)}> 
            Decline
        </Button>
    </div>;
  }

  if(store.isAccepted == true) {
    return(
      <div className="App">
        <Chat messages={store.messages} sendMessage={sendMessage} closeConnection={closeConnection} />
      </div>
    );
  }

  return (
    <div className="App">
        <h2>User: {store.user.username}</h2>
        <div className='for-button'>
        <Button className='log-out' onClick={() => store.logout()}>
            Log out
        </Button>
        </div>
        
        <input
                onChange={e => setDestination(e.target.value)} 
                value={Destination}
                type="text" 
                placeholder="destination"
            />

            <input
                onChange={e => setAddress(e.target.value)} 
                value={Address}
                type="text" 
                placeholder="address"
            />
        <Button onClick={() => store.createOrder(0 ,Destination, Address)}>
            Create Order
        </Button>

        {/* <Lobby signalRService={signalRService}/> */}
    </div>
  );
}

export default observer(App);
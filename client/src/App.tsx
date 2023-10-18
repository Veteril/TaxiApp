import React, { useContext, useEffect, useState } from 'react';
import LoginForm from './components/LoginForm';
import { Context } from './index';
import { observer } from 'mobx-react-lite';
import Lobby from './components/Lobby';
import {Button} from "react-bootstrap"

function App() {

  const {store} = useContext(Context);
  const [authChecked, setAuthChecked] = useState(false);

  useEffect(() => {
    if (!authChecked && localStorage.getItem('token')) {
      store.checkAuth();
      setAuthChecked(true);
    }
  }, [authChecked, store]);

  if(store.isLoading) {
    return(
        <div>
            Wait a second...
        </div>
    );
  }

  if(!store.isAuth) {
    return (
        <LoginForm/>
    );
  }

  return (
    <div className="App">
        <h1>User: {store.user.username}</h1>
        <Button onClick={() => store.logout()}>
            Log out
        </Button>
        <Lobby/>
    </div>
  );
}

export default observer(App);
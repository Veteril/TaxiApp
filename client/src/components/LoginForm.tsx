import React, { useContext, useState } from "react";
import { Context } from "../index";
import { observer } from "mobx-react-lite";

function LoginForm() {

    const [username, setUsername] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const {store} = useContext(Context);

    return (
        <div>
            <input
                onChange={e => setUsername(e.target.value)} 
                value={username}
                type="text" 
                placeholder="Username"
            />

            <input
                onChange={e => setPassword(e.target.value)} 
                value={password}
                type="password" 
                placeholder="Password"
            />

            <button onClick={() => store.login(username, password)}>
                Sign In
            </button>    
            <button onClick={() => store.registration(username, password)}>
                Sign Up
            </button>
        </div>
    );
}

export default observer(LoginForm);
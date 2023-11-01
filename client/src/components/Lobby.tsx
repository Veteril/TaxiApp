import { useContext, useState } from "react";
import { Context } from "../index";
import {Button, Form} from "react-bootstrap"
import { observer } from "mobx-react-lite";

function Lobby () {

    const {store} = useContext(Context);
    const [room, setRoom] = useState('');
   
    return(
        <Form className ='lobby'
            onSubmit={e => {
                e.preventDefault();
     //           SignalRService.joinRoom(store.user.username, room);
            }}
        >
            <Form.Group>
                <Form.Control 
                placeholder='room' 
                onChange={e => setRoom(e.target.value)}
                value={room} />
            </Form.Group>  
            <Button variant='success' type='submit' disabled={!room}>
                    Create Order
            </Button>
        </Form> 
    );
}

export default observer(Lobby);
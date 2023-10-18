import { useContext, useState } from "react";
import { Context } from "../index";
import {Button, Form} from "react-bootstrap"
import { observer } from "mobx-react-lite";

function Lobby () {

    const {store} = useContext(Context);
    const [room, setRoom] = useState();
   

    return(
        <Form className ='lobby'>
            <Form.Group>
                <Form.Control placeholder='name'  />
                <Form.Control placeholder='room'  />
            </Form.Group> 
            <Button variant='success' type='submit' disabled={ !room}>
                    Join
            </Button>
        </Form> 
    );
}

export default observer(Lobby);
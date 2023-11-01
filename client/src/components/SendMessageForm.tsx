import exp from "constants";
import { useState } from "react";
import {Button, Form, InputGroup} from "react-bootstrap"


function SendMessageForm({sendMessage}: { sendMessage: (message:string) => void }) {

    const [message, setMessage] = useState<string>('')

    return(
        <Form onSubmit={e => {
            e.preventDefault();
            sendMessage(message);
            setMessage('');
        }}>
            <InputGroup>
                <Form.Control placeholder="message..."
                    onChange={e => setMessage(e.target.value)} value={message} />
                    <InputGroup>
                        <Button 
                        variant="primary" 
                        type='submit'
                        disabled={!message}>
                            Send
                        </Button>
                    </InputGroup>
            </InputGroup>
        </Form>
    );
}

export default SendMessageForm;
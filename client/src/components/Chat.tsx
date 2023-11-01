import MessageContainer from "./MessageContainer";
import SendMessageForm from "./SendMessageForm";
import {Button} from "react-bootstrap"

type ChatProps = {
    messages: { user: string; message: string; }[];
    sendMessage: (message: string) => void;
    closeConnection: () => void;
  };

function Chat({ messages, sendMessage, closeConnection }: ChatProps) : JSX.Element {
    return(
    <div className="chat">
        <div className="leave-room">
            <Button variant='danger' onClick={() => closeConnection()}>
                Quit Order
            </Button>
        </div>
        <MessageContainer messages={messages} />
        <SendMessageForm sendMessage={sendMessage}/>
    </div>
    );
}

export default Chat;
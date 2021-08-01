import React, { useContext } from 'react';
import { AppContext, IAppContext } from '../../data/app-context';
import { AppMessage } from '../../data/models/app-message';

const MessagesContainer: React.FC = (props) => {
  const { actions, state } = useContext(AppContext) as IAppContext;

  const dismissMessageHandler = (message: AppMessage) => {
    actions.removeMessage(message);
  };

  const _renderMessage = (message: AppMessage, index: number): JSX.Element => {
    let className: string;
    switch (message.type) {
      case 'ERROR':
        className = 'danger';
        break;
      case 'WARN':
        className = 'warning';
        break;
      case 'INFO':
        className = 'info';
        break;
      case 'SUCCESS':
        className = 'success';
        break;
      default:
        className = 'dark';
    }

    return (
      <div key={index} className={`alert alert-${className} alert-dismissible fade show`} role="alert">
        {message.text}
        <button
          type="button"
          className="close"
          data-dismiss="alert"
          aria-label="Close"
          onClick={() => dismissMessageHandler(message)}
        >
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    );
  };

  if (!state.appMessages || !state.appMessages.length) return null;

  return (
    <div className="messages-container-component container">
      {state.appMessages.map((m, i) => _renderMessage(m, i))}
    </div>
  );
};

export default MessagesContainer;

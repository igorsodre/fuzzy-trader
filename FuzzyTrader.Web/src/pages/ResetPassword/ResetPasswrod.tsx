import React from 'react';
import { RouteComponentProps, useLocation } from 'react-router-dom';

const ResetPassword: React.FC<RouteComponentProps> = (props) => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const confirmationToken = queryParams.get('token');
  const email = queryParams.get('email');
  return (
    <div style={{ marginTop: 50 }}>
      Texto: {confirmationToken} - {email}
    </div>
  );
};

export default ResetPassword;

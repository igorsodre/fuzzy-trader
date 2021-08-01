import React, { useContext } from 'react';
import { useForm } from 'react-hook-form';
import { Link, RouteComponentProps } from 'react-router-dom';
import AppInput from '../../components/FormElements/AppInput';
import MainContainer from '../../components/UiElements/MainContainer';
import { AppContext, IAppContext } from '../../data/app-context';
import { ErrorResponse } from '../../data/contracts/responses/default-responses';
import { useAuth } from '../../hooks/auth-service';

interface FormInputs {
  email: string;
  password: string;
}
const LoginPage: React.FC<RouteComponentProps> = (props) => {
  const { actions } = useContext(AppContext) as IAppContext;
  const { register, handleSubmit, formState, errors, reset } = useForm<FormInputs>({
    defaultValues: { email: '', password: '' },
    mode: 'onChange',
  });
  const { isValid } = formState;

  const { login } = useAuth();

  const submithandler = async (data: FormInputs) => {
    const { email, password } = data;
    try {
      const result = await login(email, password);
      reset();
      actions.setAccessToken(result.accessToken);
      actions.setCurrentUser(result.user);
      props.history.replace('/home');
    } catch (err) {
      (err as ErrorResponse).errors?.forEach((e) => {
        actions.addErrorMessage(e.message);
      });
    }
  };
  return (
    <MainContainer>
      <form onSubmit={handleSubmit(submithandler)}>
        <h3>Log in</h3>

        <AppInput
          label="Email"
          register={register({
            required: true,
            pattern: {
              value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
              message: 'invalid email address',
            },
          })}
          name="email"
          type="email"
          className="form-control"
          placeholder="Enter email"
          haserror={!!errors.email}
          errortext={errors.email?.message || ''}
        />

        <AppInput
          label="Password"
          register={register({ minLength: 4 })}
          name="password"
          type="password"
          className="form-control"
          placeholder="Enter password"
          haserror={!!errors.password}
          errortext="Minimum length for password: 4"
        />

        <button disabled={!isValid} type="submit" className="btn btn-dark btn-lg btn-block">
          Sign in
        </button>
        <p className="forgot-password text-right">
          Forgot <Link to="/forgot_password">password?</Link>
        </p>
      </form>
    </MainContainer>
  );
};

export default LoginPage;

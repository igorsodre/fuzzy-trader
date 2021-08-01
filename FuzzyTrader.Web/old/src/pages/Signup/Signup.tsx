import React from 'react';
import { useForm } from 'react-hook-form';
import { Link, RouteComponentProps } from 'react-router-dom';
import AppInput from '../../components/FormElements/AppInput';
import MainContainer from '../../components/UiElements/MainContainer';
import { useAuth } from '../../hooks/auth-service';

interface FormInputs {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
}

const Signup: React.FC<RouteComponentProps> = (props) => {
  const { register, handleSubmit, formState, errors, reset, getValues } = useForm<FormInputs>({
    defaultValues: { email: '', name: '', password: '', confirmPassword: '' },
    mode: 'onChange',
  });
  const { isValid } = formState;

  const { register: signup } = useAuth();

  const submithandler = async (data: FormInputs) => {
    const { name, email, password, confirmPassword } = data;
    try {
      await signup(name, email, password, confirmPassword);
      reset();
      props.history.replace('/login');
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <MainContainer>
      <form onSubmit={handleSubmit(submithandler)}>
        <h3>Register</h3>

        <AppInput
          label="Full Name"
          register={register({
            minLength: 2,
          })}
          name="name"
          type="text"
          className="form-control"
          placeholder="Full Name"
          haserror={!!errors.name}
          errortext="Minimum length for name: 2"
        />

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
          register={register({ minLength: 6 })}
          name="password"
          type="password"
          className="form-control"
          placeholder="Enter password"
          haserror={!!errors.password}
          errortext="Minimum length for password: 6"
        />

        <AppInput
          label="Confirm password"
          register={register({
            validate: (value) => value === getValues('password'),
          })}
          name="confirmPassword"
          type="password"
          className="form-control"
          placeholder="repeat password"
          haserror={!!errors.confirmPassword}
          errortext="Value does not match with inserted password"
        />

        <button disabled={!isValid} type="submit" className="btn btn-dark btn-lg btn-block">
          Register
        </button>
        <p className="forgot-password text-right">
          Already registered <Link to="/login">log in?</Link>
        </p>
      </form>
    </MainContainer>
  );
};

export default Signup;

import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Formik, Form } from 'formik';
import { Link } from 'react-router-dom';
import { isEmail, isNullOrWhitespace } from '../../helpers/validators';
import {
    login
} from '../../store/reducers/account';

const validator = (values, errorMessages) => {
    let errors = {};

    if (!isEmail(values.email))
        errors.email = errorMessages.email || "Du skal indtaste en emailadresse.";

    if (isNullOrWhitespace(values.password))
        errors.password = errorMessages.password || "Du skal indtaste et kodeord.";
        
    return errors;
};

const Login = props => {
    const renderer = ({ touched, errors, values, handleChange, handleBlur }) => (
        <Form>
            <h1>Login</h1>

            {props.errorMessages.length > 0 && <div className="alert alert-danger">
                {props.errorMessages.map((e, i) => <div key={i}>{e}</div>)}
            </div>}

            <div className="form-group">
                <label htmlFor="email-input" style={touched.email && errors.email ? { color: "red"} : {}}>Email *</label>
                <input
                    id="email-input"
                    className="form-control"
                    onChange={handleChange}
                    onBlur={handleBlur}
                    value={values.email}
                    type="email"
                    name="email"
                    placeholder="Email"
                    required
                />
                {touched.email && errors.email && <div className="invalid-feedback">{errors.email}</div>}
            </div>
            
            <div className="form-group">
                <label htmlFor="password-input" style={touched.password && errors.password ? { color: "red"} : {}}>Kodeord *</label>
                <input
                    id="password-input"
                    className="form-control"
                    onChange={handleChange}
                    onBlur={handleBlur}
                    value={values.password}
                    type="password"
                    name="password"
                    required
                />
                {touched.password && errors.password && <div className="invalid-feedback">{errors.password}</div>}
            </div>
            
            <button className="btn btn-primary" type="submit" disabled={props.isLoading}>Login</button>

            <div className="text-center">
                <Link to="/forgot-password">Glemt kodeord?</Link>
            </div>
            <div className="text-center">
                <Link to="/register">Opret en ny konto</Link>
            </div>
        </Form>
    );
    return <div className="row">
        <div className="col-12 col-sm-6 offset-sm-3">
            <Formik initialValues={{ email: "", password: "" }}
                validate={validator}
                onSubmit={(values) => props.login(values.email, values.password)}
                render={renderer}>
            </Formik>
        </div>
    </div>;
};

const mapStateToProps = ({ account, profile }) => ({
    loggedIn: account.loggedIn,
    isLoading: account.isLoading,
    errorMessages: account.errorMessages,
    profile: profile.profile
});

const mapDispatchToProps = dispatch =>
    bindActionCreators(
        { login },
        dispatch
    );

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Login);

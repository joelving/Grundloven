import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Formik, Form } from 'formik';
import { Link } from 'react-router-dom';
import { isEmail, isNullOrWhitespace } from '../../helpers/validators';
import ProblemDetails from '../../components/problem-details';
import { login } from '../../store/reducers/account';

const validator = ({ email, password }) => {
    let errors = {};

    if (!isEmail(email))
        errors.email = "Du skal indtaste en emailadresse.";

    if (isNullOrWhitespace(password))
        errors.password = "Du skal indtaste et kodeord.";
        
    return errors;
};

const Login = ({ loggedIn, profile, request }) => {
    const { isLoading, problemDetails } = request;

    const renderer = ({ touched, errors, values, handleChange, handleBlur }) => (
        <Form>
            <h1>Log ind</h1>

            <ProblemDetails {...problemDetails} />

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
            
            <button className="btn btn-primary" type="submit" disabled={isLoading}>Login</button>

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
                onSubmit={({ email, password }) => this.props.login(email, password)}
                render={renderer}>
            </Formik>
        </div>
    </div>;
};

const mapStateToProps = ({ account }) => ({
    loggedIn: account.loggedIn,
    profile: account.profile,
    request: account.login
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

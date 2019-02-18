import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Formik, Form } from 'formik';
import { Link } from 'react-router-dom';
import { isEmail, isNullOrWhitespace } from '../../helpers/validators';
import ProblemDetails from '../../components/problem-details';
import { register } from '../../store/reducers/account';

const validator = ({ email, password, confirmPassword }) => {
    let errors = {};

    if (!isEmail(email))
        errors.email = "Du skal indtaste en emailadresse.";

    if (isNullOrWhitespace(password))
        errors.password = "Du skal indtaste et kodeord.";

    if (isNullOrWhitespace(confirmPassword))
        errors.confirmPassword = "Du skal bekræfte dit kodeord.";
    else if (password !== confirmPassword)
        errors.confirmPassword = "Begge kodeord skal være ens.";
        
    return errors;
};

const Register = ({ loggedIn, profile, request }) => {
    const { isLoading, problemDetails, succeeded } = request;

    const renderer = ({ touched, errors, values, handleChange, handleBlur }) => (
        <Form>
            <h1>Opret bruger</h1>

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
            
            <div className="form-group">
                <label htmlFor="confirm-password-input" style={touched.confirmPassword && errors.confirmPassword ? { color: "red"} : {}}>Gentag kodeord *</label>
                <input
                    id="confirm-password-input"
                    className="form-control"
                    onChange={handleChange}
                    onBlur={handleBlur}
                    value={values.confirmPassword}
                    type="password"
                    name="confirmPassword"
                    required
                />
                {touched.confirmPassword && errors.confirmPassword && <div className="invalid-feedback">{errors.confirmPassword}</div>}
            </div>
            
            <button className="btn btn-primary" type="submit" disabled={isLoading}>Opret bruger</button>

            <div className="text-center">
                <Link to="/login">Log ind med eksisterende bruger</Link>
            </div>
        </Form>
    );
    return <div className="row">
        <div className="col-12 col-sm-6 offset-sm-3">
            {succeeded ?
            <div>
                <h2>Bruger oprettet</h2>
                <p>Din bruger er blevet oprettet, og vi har sendt en mail med et link til at bekræfte din emailadresse og logge ind første gang.</p>
            </div>
            :
            <Formik initialValues={{ email: "", password: "", confirmPassword: "" }}
                validate={validator}
                onSubmit={({ email, password }) => this.props.register(email, password)}
                render={renderer}>
            </Formik>}
        </div>
    </div>;
};

const mapStateToProps = ({ account }) => ({
    loggedIn: account.loggedIn,
    profile: account.profile,
    request: account.register
});

const mapDispatchToProps = dispatch =>
    bindActionCreators(
        { register },
        dispatch
    );

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Register);

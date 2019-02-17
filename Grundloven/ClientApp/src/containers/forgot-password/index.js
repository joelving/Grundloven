import React from 'react';
//import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Formik, Form } from 'formik';
import { Link } from 'react-router-dom';
import { isEmail } from '../../helpers/validators';
import ProblemDetails from '../../components/problem-details';
// import {
//     forgotPassword
// } from '../../store/reducers/account';

const validator = (values, errorMessages) => {
    let errors = {};

    if (!isEmail(values.email))
        errors.email = errorMessages.email || "Du skal indtaste en emailadresse.";
        
    return errors;
};

class ForgotPassword extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            problemDetails: null,
            isLoading: false,
            showReceipt: false
        };
    }

    forgotPassword = async email => {
        try {
            await post('/api/account/forgot-password', { email });
            this.setState(state => state.showReceipt = true);
        }
        catch (problemDetails) {
            this.setState(state => state.problemDetails = problemDetails);
        }
    }

    render() {
        const { isLoading, problemDetails, showReceipt } = this.state;
        const renderer = ({ touched, errors, values, handleChange, handleBlur }) => (
            <Form>
                <h1>Glemt kodeord</h1>

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
                
                <button className="btn btn-primary" type="submit" disabled={isLoading}>Indsend</button>

                <div className="text-center">
                    <Link to="/login">Log ind med eksisterende bruger</Link>
                </div>
                <div className="text-center">
                    <Link to="/register">Opret en ny konto</Link>
                </div>
            </Form>
        );
        return <div className="row">
            <div className="col-12 col-sm-6 offset-sm-3">
                {showReceipt ?
                <div>
                    <h2>Forespørgsel indsendt</h2>
                    <p>Vi har modtaget din anmodning om at få nulstillet dit kodeord. Du bør modtage en email med et link til at nulstille dit kodeord snarest.</p>
                </div>
                :
                <Formik initialValues={{ email: "" }}
                    validate={validator}
                    onSubmit={({ email }) => this.forgotPassword(email)}
                    render={renderer}>
                </Formik>}
            </div>
        </div>;
    }
};

const mapStateToProps = ({ account }) => ({
    loggedIn: account.loggedIn,
});

// const mapDispatchToProps = dispatch =>
//     bindActionCreators(
//         { forgotPassword },
//         dispatch
//     );

export default connect(
    mapStateToProps,
    //mapDispatchToProps
)(ForgotPassword);

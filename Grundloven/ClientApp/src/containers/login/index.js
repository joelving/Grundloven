import React from 'react';
import { push } from 'connected-react-router';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import {
    login
} from '../../store/reducers/account';

const Home = props => (
    <div>
        <h1>Login</h1>
    </div>
);

const mapStateToProps = ({ account, profile }) => ({
    loggedIn: account.loggedIn,
    isLoading: account.isLoading,
    errorMessages: account.errorMessages,
    profile: profile.profile
});

const mapDispatchToProps = dispatch =>
    bindActionCreators(
        {
            login,
            goToRegister: () => push('/register'),
            goToForgotPassword: () => push('/forgot-password')
        },
        dispatch
    );

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Home);

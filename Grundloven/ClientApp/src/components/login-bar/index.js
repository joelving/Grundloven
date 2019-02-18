import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { logout } from '../../store/reducers/account';

const Aux = props => props.children;

const LoginMenu = () => <Aux>
        <Link className="nav-link" to="/login">Log ind</Link>
        <Link className="nav-link" to="/register">Opret bruger</Link>
    </Aux>;

const LoggedInBar = ({ profile, logout }) => <Aux>
        <Link to="/profile/me">Hej {profile.username}</Link>
        <Link onClick={() => logout()}>Log ud</Link>
    </Aux>;

const LoginBar = ({ loggedIn, profile, logout }) => loggedIn ? <LoggedInBar profile={profile} logout={logout} /> : <LoginMenu />;

const mapStateToProps = ({ account }) => ({
    loggedIn: account.loggedIn,
    profile: account.profile
});

const mapDispatchToProps = dispatch =>
    bindActionCreators(
        { logout },
        dispatch
    );

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(LoginBar);
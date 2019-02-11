import React from 'react';
import { push } from 'connected-react-router';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import {
    increment,
    incrementAsync,
    decrement,
    decrementAsync
} from '../../store/reducers/counter';

const Home = props => (
    <div>
        <h1>Home</h1>
    </div>
);

const mapStateToProps = () => ({
});

const mapDispatchToProps = dispatch =>
    bindActionCreators(
        {
        },
        dispatch
    );

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Home);

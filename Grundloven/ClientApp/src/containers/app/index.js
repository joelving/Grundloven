import React from 'react';
import { Route, Link } from 'react-router-dom';
import Home from '../home';
import Login from '../login';
import Register from '../register';

const App = () => (
    <div>
        <header>
            <Link to="/">Home</Link>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
        </header>

        <main>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/register" component={Register} />
        </main>
    </div>
);

export default App;

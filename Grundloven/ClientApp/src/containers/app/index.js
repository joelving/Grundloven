import React from 'react';
import { Route, Link } from 'react-router-dom';
import Home from '../home';
import Login from '../login';

const App = () => (
    <div>
        <header>
            <Link to="/">Home</Link>
            <Link to="/login">Login</Link>
        </header>

        <main>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
        </main>
    </div>
);

export default App;

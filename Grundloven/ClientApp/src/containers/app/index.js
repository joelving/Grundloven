import React from 'react';
import { Route, Link } from 'react-router-dom';

import LoginBar from '../../components/login-bar';

import Home from '../home';
import Login from '../login';
import Register from '../register';
import EmailConfirmed from '../email-confirmed';

const App = () => (
    <div>
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <Link className="navbar-brand" to="/">Grundlov.nu</Link>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbar" aria-controls="navbar" aria-expanded="false" aria-label="Åben navigationsmenu">
                <span className="navbar-toggler-icon"></span>
            </button>

            <div className="collapse navbar-collapse" id="navbar">
                <ul className="navbar-nav mr-auto mt-2 mt-lg-0">
                    <li className="nav-item active">
                        <Link className="nav-link" to="/">Forsiden <span className="sr-only">(nuværende)</span></Link>
                    </li>
                </ul>

                <div className="form-inline my-2 my-lg-0">
                    <LoginBar />
                </div>
            </div>
        </nav>

        <main>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/register" component={Register} />
            <Route exact path="/email-confirmed" component={EmailConfirmed} />
        </main>
    </div>
);

export default App;

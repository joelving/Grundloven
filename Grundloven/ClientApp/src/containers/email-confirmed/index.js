import React from 'react';
import { Link } from 'react-router-dom';

const EmailConfirmed = () => (
    <div>
        <h1>Emailadresse bekræftet</h1>
        <p>Din emailadresse er blevet bekræftet.</p>
        <p>Gå til <Link to="/profile/me">din profil</Link>.</p>
    </div>
);

export default EmailConfirmed;

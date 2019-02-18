import React from 'react';

const ProblemDetails = ({ title, detail, errors }) => !title ? null :
    <div className="alert alert-danger">
        <p><strong>{title}</strong></p>
        {detail && <p>{detail}</p>}
        {errors && errors.length > 0 && <ul>
            {errors.map(e => <li>{e.description}</li>)}
        </ul>}
    </div>;

export default ProblemDetails;

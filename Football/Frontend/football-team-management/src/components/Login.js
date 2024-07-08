import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import '../styles/Login.css';

const Login = () => {
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [errorMessage, setErrorMessage] = useState('');
    const { login, user } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
        if (user) {
            if (user.role === 'Secretary') {
                navigate('/secretary');
            } else if (user.role === 'Coach') {
                navigate('/trainer');
            } else {
                navigate('/player');
            }
        }
    }, [user, navigate]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const loggedInUser = await login(formData.email, formData.password);
            if (loggedInUser.role === 'Secretary') {
                navigate('/secretary');
            } else if (loggedInUser.role === 'Coach') {
                navigate('/trainer');
            } else {
                navigate('/player');
            }
        } catch (error) {
            console.error('Error during login', error);
            setErrorMessage('Incorrect email or password. Please try again.');
        }
    };

    return (
        <div className="login-container">
            <form className="login-form" onSubmit={handleSubmit}>
                <h1>Log in!</h1>
                <img src="logo.png" alt="Logo" />
                <input type="email" name="email" placeholder="Email" onChange={handleChange} required />
                <input type="password" name="password" placeholder="Password" onChange={handleChange} required />
                <button type="submit">Login</button>
                {errorMessage && <p className="error-message">{errorMessage}</p>}
                <p>
                    Don't have an account? <Link to="/register">Sign Up</Link>
                </p>
            </form>
        </div>
    );
};

export default Login;

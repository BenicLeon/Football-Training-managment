import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import '../styles/Login.css';

const Register = () => {
    const [formData, setFormData] = useState({
        userName: '',
        email: '',
        password: '',
        confirmPassword: ''
    });
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const { register, user } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
        if (user) {
            if (user.role === 'Secretary') {
                navigate('/secretary');
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
        setErrorMessage('');
        setSuccessMessage('');

        if (formData.password !== formData.confirmPassword) {
            setErrorMessage('Passwords do not match. Please try again.');
            return;
        }

        try {
            await register(formData);
            setSuccessMessage('Registration successful. You can now log in.');
            setTimeout(() => {
                navigate('/login');
            }, 1000);
        } catch (error) {
            console.error('Error during registration', error);
            setErrorMessage('Registration failed: ' + (error.response?.data?.message || error.message));
        }
    };

    return (
        <div className="login-container">
            <form className="register-form" onSubmit={handleSubmit}>
                <h1>Register!</h1>
                <img src="logo.png" alt="Logo" /> 
                <input type="text" name="userName" placeholder="Username" onChange={handleChange} required />
                <input type="email" name="email" placeholder="Email" onChange={handleChange} required />
                <input type="password" name="password" placeholder="Password" onChange={handleChange} required />
                <input type="password" name="confirmPassword" placeholder="Confirm Password" onChange={handleChange} required />
                <button type="submit">Register</button>
                {errorMessage && <p className="error-message">{errorMessage}</p>}
                {successMessage && <p className="success-message">{successMessage}</p>}
                <p>
                    Already have an account? <Link to="/login">Login here</Link>
                </p>
            </form>
        </div>
    );
};

export default Register;

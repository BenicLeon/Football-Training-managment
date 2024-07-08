import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../styles/ChangePassword.css';

const ChangePassword = () => {
    const [formData, setFormData] = useState({
        newPassword: '',
        confirmPassword: ''
    });
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.newPassword !== formData.confirmPassword) {
            setErrorMessage('Passwords do not match. Please try again.');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            await axios.put('/api/Auth/change-password', formData, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setSuccessMessage('Password changed successfully.');
            setErrorMessage('');
        } catch (error) {
            console.error('Error changing password', error);
            setErrorMessage('Failed to change password. Please try again.');
            setSuccessMessage('');
        }
    };

    const handleLoginRedirect = () => {
        navigate('/login');
    };

    const handleBack = () => {
        navigate(-1);
    };

    return (
        <div className="change-password-container">
            <form className="change-password-form" onSubmit={handleSubmit}>
                <input
                    type="password"
                    name="newPassword"
                    placeholder="New Password"
                    onChange={handleChange}
                    required
                />
                <input
                    type="password"
                    name="confirmPassword"
                    placeholder="Confirm Password"
                    onChange={handleChange}
                    required
                />
                <div className="button-group">
                    <button type="submit">Change Password</button>
                    <button type="button" onClick={handleBack} className="btn-back">Back</button>
                </div>
                {errorMessage && <p className="error-message">{errorMessage}</p>}
                {successMessage && (
                    <div className="success-message">
                        <p>{successMessage}</p>
                        <button type="button" onClick={handleLoginRedirect}>Go back</button>
                    </div>
                )}
            </form>
        </div>
    );
};

export default ChangePassword;

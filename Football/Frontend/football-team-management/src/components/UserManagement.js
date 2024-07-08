import React, { useEffect, useState, useCallback } from 'react';
import axios from 'axios';
import {jwtDecode} from 'jwt-decode';
import '../styles/UserManagement.css';

const secretaryEmail = "tajniklukic@gmail.com";

const UserManagement = () => {
    const [users, setUsers] = useState([]);

    const fetchCurrentUser = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            if (token) {
                const decodedToken = jwtDecode(token);
                console.log('Current user email:', decodedToken.email); 
            }
        } catch (error) {
            console.error('Error fetching current user', error);
        }
    }, []);

    const fetchUsers = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Auth/users', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setUsers(response.data);
        } catch (error) {
            console.error('Error fetching users', error);
        }
    }, []);

    useEffect(() => {
        fetchCurrentUser();
        fetchUsers();
    }, [fetchCurrentUser, fetchUsers]);

    const deleteUser = async (userId) => {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/Auth/users/${userId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setUsers(users.filter(user => user.id !== userId));
        } catch (error) {
            console.error('Error deleting user', error);
        }
    };

    return (
        <div className="user-management-container">
            <h1>Manage Users</h1>
            <div className="user-table-container">
                <table className="user-table">
                    <thead>
                        <tr>
                            <th>Email</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user) => (
                            <tr key={user.id}>
                                <td>{user.email}</td>
                                <td>
                                    {user.email !== secretaryEmail && (
                                        <button className="btn-remove" onClick={() => deleteUser(user.id)}>
                                            <i className="fas fa-trash"></i>
                                        </button>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default UserManagement;

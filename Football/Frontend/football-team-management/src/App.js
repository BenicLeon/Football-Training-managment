import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Register from './components/Register';
import Login from './components/Login';
import SecretaryDashboard from './components/SecretaryDashboard';
import TrainerDashboard from './components/TrainerDashboard';
import PlayerDashboard from './components/PlayerDashboard';
import TrainingSessionDetails from './components/TrainingSessionDetails';
import PlayerPrivateTrainings from './components/PlayerPrivateTrainings';
import { AuthProvider } from './context/AuthContext';
import PrivateRoute from './components/PrivateRoute';
import ChangePassword from './components/ChangePassword';
import Home from './components/Home';

const App = () => {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/" element={<PrivateRoute><Home /></PrivateRoute>} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/secretary/*" element={<PrivateRoute role="Secretary"><SecretaryDashboard /></PrivateRoute>} />
                    <Route path="/trainer/*" element={<PrivateRoute role="Coach"><TrainerDashboard /></PrivateRoute>} />
                    <Route path="/player/*" element={<PrivateRoute role="Player"><PlayerDashboard /></PrivateRoute>} />
                    <Route path="/trainer/trainings/:id" element={<PrivateRoute role="Coach"><TrainingSessionDetails /></PrivateRoute>} />
                    <Route path="/trainer/team-players/:playerId/private-trainings" element={<PrivateRoute role="Coach"><PlayerPrivateTrainings /></PrivateRoute>} />
                    <Route path="/change-password" element={<PrivateRoute><ChangePassword /></PrivateRoute>} />
                </Routes>
            </Router>
        </AuthProvider>
    );
};

export default App;

import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import PlayerList from './components/PlayerList';
import AddPlayer from './components/AddPlayer';
import EditPlayer from './components/EditPlayer';
import './App.css';

const App = () => {
  return (
    <Router>
      <Navbar />
      <div className="container">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/players" element={<PlayerList />} />
          <Route path="/add-player" element={<AddPlayer />} />
          <Route path="/edit-player/:id" element={<EditPlayer />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;

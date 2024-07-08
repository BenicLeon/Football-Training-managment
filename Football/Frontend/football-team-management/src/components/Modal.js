import React from 'react';
import '../styles/Modal.css'; 

const Modal = ({ show, handleClose, children }) => {
    const showHideClassName = show ? "modal display-block" : "modal display-none";

    return (
        <div className={showHideClassName}>
            <div className="modal-main">
                {children}
                <button className="modal-close" onClick={handleClose}>Close</button>
            </div>
        </div>
    );
};

export default Modal;

/**
 * Blazouter JavaScript Interop
 * 
 * This module provides TypeScript-based JavaScript interop functionality for Blazouter.
 * It includes utilities for browser navigation, document manipulation, and more.
 * 
 * @packageDocumentation
 */

// Export all navigation utilities
export * from './navigation.js';

// Export all document utilities
export * from './document.js';

// Export all storage utilities
export * from './storage.js';

// Export all viewport utilities
export * from './viewport.js';

// Export all clipboard utilities
export * from './clipboard.js';

/**
 * Main Blazouter interop namespace
 */
export const Blazouter = {
    version: '1.0.13',
    
    /**
     * Initializes Blazouter JavaScript interop
     */
    initialize(): void {
        console.log('Blazouter JS Interop v' + this.version + ' initialized');
    }
};

// Auto-initialize when the script loads
if (typeof window !== 'undefined') {
    Blazouter.initialize();
    
    // Make the main namespace available globally
    (window as any).Blazouter = Blazouter;
}